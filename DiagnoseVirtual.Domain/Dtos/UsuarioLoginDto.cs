using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
