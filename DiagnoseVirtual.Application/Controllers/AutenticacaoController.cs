using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Domain.Entities;
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
        private readonly UsuarioService sUsuario = new UsuarioService();
        private readonly IConfiguration config;

        public AutenticacaoController(IConfiguration config)
        {
            this.config = config;
        }

        [HttpPost("Register")]
        public ActionResult Register(UsuarioRegistroDto novoUsuarioDto)
        {
            if (sUsuario.ExisteUsuario(novoUsuarioDto.Cpf))
                return BadRequest("Já existe um cadastro com este CPF");

            var novoUsuario = sUsuario.Cadastrar(novoUsuarioDto);

            return StatusCode(201);
        }

        [HttpPost("Login")]
        public ActionResult Login(UsuarioLoginDto usuarioLoginDto)
        {
            var usuario = sUsuario.Login(usuarioLoginDto.Cpf, usuarioLoginDto.Password);

            if (usuario == null)
                return Unauthorized();

            var token = TokenHelper.GerarTokenUsuario(usuario, config.GetSection("AppSettings:Token").Value);

            return Ok(new { token });
        }
    }
}
