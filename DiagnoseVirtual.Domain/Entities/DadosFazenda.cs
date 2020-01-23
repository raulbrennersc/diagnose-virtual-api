using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Entities
{
    public class DadosFazenda : BaseEntity
    {
        public string Cultura { get; set; }
        public double AreaTotal { get; set; }
        public int QuantidadeLavouras { get; set; }
        public Fazenda Fazenda { get; set; }
        public int IdFazenda { get; set; }
    }
}
