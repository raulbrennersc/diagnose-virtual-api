using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class UsuarioRegistroDto
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Cpf { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "A senha deve conter no mínimo 12 caractéres")]
        public string Password { get; set; }
    }
}
