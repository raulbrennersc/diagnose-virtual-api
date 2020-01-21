using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class DadosFazendaDto
    {
        public string Cultura { get; set; }
        public double AreaTotal { get; set; }
        public int QuantidadeLavouras { get; set; }

        public DadosFazendaDto() { }
        public DadosFazendaDto(DadosFazenda dados)
        {
            Cultura = dados.Cultura;
            AreaTotal = dados.AreaTotal;
            QuantidadeLavouras = dados.QuantidadeLavouras;
        }
    }
}
