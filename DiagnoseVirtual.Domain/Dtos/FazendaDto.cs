using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class FazendaDto
    {
        public int Id { get; set; }
        public LocalizacaoFazendaDto LocalizacaoFazenda { get; set; }
        public DadosFazendaDto DadosFazenda { get; set; }
        public LocalizacaoGeoDto LocalizacaoGeo { get; set; }
        public List<LavouraDto> Lavouras { get; set; }
        public bool Ativa { get; set; }
        public bool Concluida { get; set; }

        public FazendaDto() { }
        public FazendaDto(Fazenda fazenda)
        {
            Id = fazenda.Id;
            LocalizacaoFazenda = fazenda.LocalizacaoFazenda != null ? new LocalizacaoFazendaDto(fazenda.LocalizacaoFazenda) : null;
            DadosFazenda = fazenda.DadosFazenda != null ? new DadosFazendaDto(fazenda.DadosFazenda) : null;
            LocalizacaoGeo = fazenda.LocalizacaoGeo != null ? new LocalizacaoGeoDto(fazenda.LocalizacaoGeo) : null;
            Lavouras = fazenda.Lavouras != null ? fazenda.Lavouras.Select(l => new LavouraDto(l)).ToList() : null;
            Ativa = fazenda.Ativa;
            Concluida = fazenda.Concluida;
        }
    }
}
