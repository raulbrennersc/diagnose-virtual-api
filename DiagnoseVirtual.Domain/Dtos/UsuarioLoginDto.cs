using System.ComponentModel.DataAnnotations;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class UsuarioLoginDto
    {
        [Required]
        public string Cpf { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
