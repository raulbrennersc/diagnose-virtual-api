using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Entities
{
    public class DadosFazenda
    {
        public int Id { get; set; }
        public string Cultura { get; set; }
        public double AreaTotal { get; set; }
        public int QuantidadeLavouras { get; set; }
    }
}
