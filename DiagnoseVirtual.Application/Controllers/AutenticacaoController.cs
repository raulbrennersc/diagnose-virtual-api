using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Service.Business;
using DiagnoseVirtual.Service.Helpers;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DiagnoseVirtual.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly BaseService<Usuario> sUsuario = new BaseService<Usuario>();
        private readonly IConfiguration config;

        public AutenticacaoController(IConfiguration config)
        {
            this.config = config;
        }

        [HttpPost("Register")]
        public ActionResult Register(UsuarioRegistroDto novoUsuarioDto)
        {
            var bUsuario = new BUsuario(sUsuario);
            if (bUsuario.ExisteUsuario(novoUsuarioDto.Cpf))
                return BadRequest("Já existe um cadastro com este CPF");

            var novoUsuario = bUsuario.Cadastrar(novoUsuarioDto);

            return StatusCode(201);
        }

        [HttpPost("Login")]
        public ActionResult Login(UsuarioLoginDto usuarioLoginDto)
        {
            var bUsuario = new BUsuario(sUsuario);
            var usuario = bUsuario.Login(usuarioLoginDto.Cpf, usuarioLoginDto.Password);

            if (usuario == null)
                return Unauthorized();

            var token = TokenHelper.GerarTokenUsuario(usuario, config.GetSection("AppSettings:Token").Value);

            return Ok(new { token });
        }
    }
}
