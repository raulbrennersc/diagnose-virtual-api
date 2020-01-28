using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Infra.Data.Context;
using DiagnoseVirtual.Service.Helpers;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;

namespace DiagnoseVirtual.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly PsqlContext _context = new PsqlContext();
        private readonly IConfiguration config;

        public AuthController(IConfiguration config)
        {
            this.config = config;
            _usuarioService = new UsuarioService(_context);
        }

        [HttpPost("Register")]
        public ActionResult Register(UsuarioRegistroDto novoUsuarioDto)
        {
            if (_usuarioService.ExisteUsuario(novoUsuarioDto.Cpf))
                return BadRequest(Constants.ERR_CPF_CADASTRADO);
            using (var transaction = _context.Database.BeginTransaction())
            {
                _usuarioService.Cadastrar(novoUsuarioDto);
                try
                {
                    transaction.Commit();
                    return StatusCode((int)HttpStatusCode.Created);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }

        [HttpPost("Login")]
        public ActionResult Login(UsuarioLoginDto usuarioLoginDto)
        {
            var usuario = _usuarioService.Login(usuarioLoginDto.Cpf, usuarioLoginDto.Password);

            if (usuario == null)
                return Unauthorized();

            var token = TokenHelper.GerarTokenUsuario(usuario, config.GetSection("AppSettings:Token").Value);

            return Ok(new { token });
        }
    }
}
