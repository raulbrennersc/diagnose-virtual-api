using System;
using System.Collections.Generic;
using System.Text;

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
            Demarcacao = lavoura.Demarcacao != null ? new GeometriaDto(lavoura.Demarcacao) : null;
            Talhoes = lavoura.Talhoes != null ? lavoura.Talhoes.Select(t => new GeometriaDto(t)).ToList() : null;
            Concluida = lavoura.Concluida;
        }
    }
}
