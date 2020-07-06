using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Infra.Data.Context;
using DiagnoseVirtual.Service.Helpers;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MimeKit;
using DiagnoseVirtual.Application.Helpers;

namespace DiagnoseVirtual.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly PsqlContext _context;
        private readonly IConfiguration config;

        public AuthController(IConfiguration config, PsqlContext context)
        {
            _context = context;
            this.config = config;
            _usuarioService = new UsuarioService(_context);
        }

        [HttpPost("Register")]
        public ActionResult Register(UsuarioRegistroDto novoUsuarioDto)
        {
            if (_usuarioService.ExisteUsuario(novoUsuarioDto.Cpf))
            {
                return BadRequest(Constants.ERR_CPF_CADASTRADO);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _usuarioService.Cadastrar(novoUsuarioDto);
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
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        public ActionResult Login(UsuarioLoginDto usuarioLoginDto)
        {
            var usuario = _usuarioService.Login(usuarioLoginDto.Cpf, usuarioLoginDto.Password);

            if (usuario == null)
            {
                return Unauthorized("Login e/ou senha incorretos!");
            }

            var token = TokenHelper.GerarTokenUsuario(usuario, config.GetSection("AppSettings:Token").Value);

            return Ok(new AuthDto
            {
                Token = token,
                Nome = usuario.Nome,
                PrimeiroAcesso = usuario.PrimeiroAcesso
            });
        }

        [HttpPut("AceitarTermo")]
        public ActionResult AceitarTermo(UsuarioLoginDto usuarioLogin)
        {
            var usuario = _usuarioService.Login(usuarioLogin.Cpf, usuarioLogin.Password);

            if (!usuario.PrimeiroAcesso)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ResponseHelper.Create(Constants.ERR_INCONFORMIDADE));
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    usuario.PrimeiroAcesso = false;
                    _usuarioService.Put(usuario);
                    transaction.Commit();
                    var token = TokenHelper.GerarTokenUsuario(usuario, config.GetSection("AppSettings:Token").Value);
                    var authDto = new AuthDto
                    {
                        Token = token,
                        Nome = usuario.Nome,
                        PrimeiroAcesso = usuario.PrimeiroAcesso
                    };
                    return StatusCode((int)HttpStatusCode.OK, ResponseHelper.Create("Termo aceito com sucesso.", authDto));
                }
                catch
                {
                    transaction.Rollback();
                    return StatusCode((int)HttpStatusCode.BadRequest, ResponseHelper.Create(Constants.ERR_INCONFORMIDADE));
                }
            }
        }

        [HttpPost("ResetPassword/{cpf}")]
        public ActionResult ResetPassword(string cpf)
        {
            var usuario = _usuarioService.GetByCpf(cpf);

            if (usuario == null)
            {
                return BadRequest("Não existe nenhum usuário com este CPF.");
            }

            var novaSenha = "novasenha";
            using(var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _usuarioService.ResetarSenha(usuario, novaSenha);
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Diagnosys", "qipixel2@gmail.com"));
            message.To.Add(new MailboxAddress(usuario.Nome, usuario.Email));
            message.Subject = "Recuperação de senha";
            var body = new BodyBuilder();
            body.HtmlBody = $"<p>Prezado(a) {usuario.Nome}, segue a nova senha que poderá ser utilizada para acessar o Diagnosys.</p>" +
                $"<p>Nova senha: {novaSenha}.</p>";
            message.Body = body.ToMessageBody();
            new EmailService().SendEmail(message);

            return Ok("Senha resetada com sucesso.");
        }
    }
}
