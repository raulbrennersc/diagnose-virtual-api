using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Service.Services;
using System.Linq;

namespace DiagnoseVirtual.Service.Business
{
    public class BUsuario
    {
        private readonly BaseService<Usuario> sUsuario;

        public BUsuario (BaseService<Usuario> sUsuario)
        {
            this.sUsuario = sUsuario;
        }

        public bool ExisteUsuario (string cpf)
        {
            return sUsuario.GetAll().Any(u => u.Cpf == cpf);
        }

        public Usuario Cadastrar(UsuarioRegistroDto novoUsuarioDto)
        {
            var novoUsuario = new Usuario
            {
                Nome = novoUsuarioDto.Nome,
                Cpf = novoUsuarioDto.Cpf,
                Email = novoUsuarioDto.Email,
            };

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(novoUsuarioDto.Password, out passwordHash, out passwordSalt);

            novoUsuario.PasswordHash = passwordHash;
            novoUsuario.PasswordSalt = passwordSalt;

            sUsuario.Post(novoUsuario);

            return novoUsuario;
        }

        public Usuario Login(string cpf, string password)
        {
            var usuario = sUsuario.GetAll().FirstOrDefault(u => u.Cpf == cpf);

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
