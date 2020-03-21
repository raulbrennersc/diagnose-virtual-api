using DiagnoseVirtual.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class LavouraDto
    {
        public int Id { get; set; }
        public DadosLavouraDto DadosLavoura { get; set; }
        public GeometriaDto Demarcacao { get; set; }
        public ICollection<GeometriaDto> Talhoes { get; set; }
        public bool Concluida { get; set; }

        public LavouraDto() { }
        public LavouraDto(Lavoura lavoura)
        {
            Id = lavoura.Id;
            DadosLavoura = lavoura.DadosLavoura != null ? new DadosLavouraDto(lavoura.DadosLavoura) : null;
            Demarcacao = new GeometriaDto(lavoura.Demarcacao);
            Talhoes = lavoura.Talhoes?.Select(t => new GeometriaDto(t.Geometria)).ToList();
            Concluida = lavoura.Concluida;
        }
    }
}
