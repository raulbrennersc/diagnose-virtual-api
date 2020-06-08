using DiagnoseVirtual.Domain.Entities;
using NetTopologySuite.Geometries;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class ProblemaMonitoramentoDto
    {
        public string Descricao { get; set; }
        public string Recomendacao { get; set; }
        public Geometry Ponto { get; set; }
        public string Imagens { get; set; }

        public ProblemaMonitoramentoDto() { }

        public ProblemaMonitoramentoDto(ProblemaMonitoramento problema)
        {
            Descricao = problema.Descricao;
            Recomendacao = problema.Recomendacao;
            Ponto = problema.Ponto;
        }
    }
}