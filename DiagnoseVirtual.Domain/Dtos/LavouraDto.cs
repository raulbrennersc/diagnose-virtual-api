using DiagnoseVirtual.Domain.Entities;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Linq;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class LavouraDto
    {
        public int Id { get; set; }
        public int IdEtapa { get; set; }
        public DadosLavouraDto DadosLavoura { get; set; }
        public Geometry Demarcacao { get; set; }
        public ICollection<Geometry> Talhoes { get; set; }
        public bool Concluida { get; set; }

        public LavouraDto() { }
        public LavouraDto(Lavoura lavoura)
        {
            Id = lavoura.Id;
            IdEtapa = lavoura.Etapa.Id;
            DadosLavoura = lavoura.DadosLavoura != null ? new DadosLavouraDto(lavoura.DadosLavoura) : null;
            Demarcacao = lavoura.Demarcacao;
            Talhoes = lavoura.Talhoes?.Select(t => t.Geometria).ToList();
            Concluida = lavoura.Concluida;
        }
    }
}
