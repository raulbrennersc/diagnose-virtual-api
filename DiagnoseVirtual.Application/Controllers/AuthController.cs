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
                return Unauthorized();
            }

            var token = TokenHelper.GerarTokenUsuario(usuario, config.GetSection("AppSettings:Token").Value);

            return Ok(new AuthDto
            {
                Token = token,
                Nome = usuario.Nome,
                PrimeiroAcesso = usuario.PrimeiroAcesso
            });
        }

        [HttpPost("AceitarTermo")]
        [Authorize]
        public ActionResult AceitarTermo()
        {
            var idUsuario = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var usuario = _usuarioService.Get(int.Parse(idUsuario));

            if (!usuario.PrimeiroAcesso)
            {
                return BadRequest();
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    usuario.PrimeiroAcesso = false;

                    _usuarioService.Put(usuario);
                    transaction.Commit();
                    return Ok();
                }
                catch
                {
                    transaction.Rollback();
                    return BadRequest();
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
