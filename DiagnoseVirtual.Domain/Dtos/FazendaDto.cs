using DiagnoseVirtual.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class FazendaDto
    {
        public int Id { get; set; }
        public LocalizacaoFazendaDto LocalizacaoFazenda { get; set; }
        public DadosFazendaDto DadosFazenda { get; set; }
        public DemarcacaoDto LocalizacaoGeo { get; set; }
        public List<LavouraDto> Lavouras { get; set; }
        public bool Ativa { get; set; }
        public bool Concluida { get; set; }

        public FazendaDto() { }
        public FazendaDto(Fazenda fazenda)
        {
            var demarcacao = new DemarcacaoDto();
            demarcacao.Geometrias = new List<GeometriaDto>();
            if(fazenda.Demarcacao != null)
                demarcacao.Geometrias.Add(new GeometriaDto(fazenda.Demarcacao));

            Id = fazenda.Id;
            LocalizacaoFazenda = fazenda.LocalizacaoFazenda != null ? new LocalizacaoFazendaDto(fazenda.LocalizacaoFazenda) : null;
            DadosFazenda = fazenda.DadosFazenda != null ? new DadosFazendaDto(fazenda.DadosFazenda) : null;
            Lavouras = fazenda.Lavouras?.Select(l => new LavouraDto(l)).ToList();
            LocalizacaoGeo = demarcacao;
            Ativa = fazenda.Ativa;
            Concluida = fazenda.Concluida;
        }
    }
}
