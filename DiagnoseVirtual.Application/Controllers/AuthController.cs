using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Service.Helpers;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NHibernate;
using System;
using System.Net;

namespace DiagnoseVirtual.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly IConfiguration config;
        private readonly ISession _session;

        public AuthController(IConfiguration config, ISession session)
        {
            this.config = config;
            _session = session;
            _usuarioService = new UsuarioService(session);
        }

        [HttpPost("Register")]
        public ActionResult Register(UsuarioRegistroDto novoUsuarioDto)
        {
            if (_usuarioService.ExisteUsuario(novoUsuarioDto.Cpf))
                return BadRequest(Constants.ERR_CPF_CADASTRADO);
            using(var transaction = _session.BeginTransaction())
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
