using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Infra.Data.Context;
using System.Linq;

namespace DiagnoseVirtual.Service.Services
{
    public class UsuarioService : BaseService<Usuario>
    {

        public UsuarioService(PsqlContext context) : base(context) { }
        public bool ExisteUsuario(string cpf)
        {
            return GetAll().Any(u => u.Cpf == cpf);
        }

        public Usuario Cadastrar(UsuarioRegistroDto novoUsuarioDto)
        {
            var novoUsuario = new Usuario
            {
                Nome = novoUsuarioDto.Nome,
                Cpf = novoUsuarioDto.Cpf.Replace(".", "").Replace("-", ""),
                Email = novoUsuarioDto.Email,
            };

            CreatePasswordHash(novoUsuarioDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            novoUsuario.PasswordHash = passwordHash;
            novoUsuario.PasswordSalt = passwordSalt;

            Post(novoUsuario);

            return novoUsuario;
        }

        public Usuario Login(string cpf, string password)
        {
            cpf = cpf.Replace(".", "").Replace("-", "");
            var usuario = GetAll().FirstOrDefault(u => u.Cpf == cpf);

            if (usuario == null)
                return null;

            if (!VerifyPasswordHash(password, usuario.PasswordHash, usuario.PasswordSalt))
                return null;

            return usuario;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }

                return true;
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
