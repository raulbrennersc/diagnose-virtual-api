using DiagnoseVirtual.Domain.Entities;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class ProblemaMonitoramentoDto
    {
        public string Descricao { get; set; }
        public string Recomendacao { get; set; }
        public GeometriaDto Ponto { get; set; }

        public ProblemaMonitoramentoDto(){}

        public ProblemaMonitoramentoDto(ProblemaMonitoramento problema)
        {
            Descricao = problema.Descricao;
            Recomendacao = problema.Recomendacao;
            Ponto = new GeometriaDto(problema.Ponto);
        }
    }
}