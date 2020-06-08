using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class AuthDto
    {
        public string Token { get; set; }
        public string Nome { get; set; }
        public bool PrimeiroAcesso { get; set; }
    }
}
