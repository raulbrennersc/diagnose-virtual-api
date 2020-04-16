using DiagnoseVirtual.Domain.Entities;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class DadosFazendaDto
    {
        public int IdCultura { get; set; }
        public double AreaTotal { get; set; }
        public int QuantidadeLavouras { get; set; }

        public DadosFazendaDto() { }
        public DadosFazendaDto(DadosFazenda dados)
        {
            IdCultura = dados.Cultura?.Id ?? 0;
            AreaTotal = dados.AreaTotal;
            QuantidadeLavouras = dados.QuantidadeLavouras;
        }
    }
}
