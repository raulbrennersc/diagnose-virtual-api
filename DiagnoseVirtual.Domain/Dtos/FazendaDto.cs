using DiagnoseVirtual.Domain.Entities;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Linq;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class FazendaDto
    {
        public int Id { get; set; }
        public int IdEtapa { get; set; }
        public LocalizacaoFazendaDto LocalizacaoFazenda { get; set; }
        public DadosFazendaDto DadosFazenda { get; set; }
        public Geometry Demarcacao { get; set; }
        public bool Ativa { get; set; }
        public bool Concluida { get; set; }

        public FazendaDto() { }
        public FazendaDto(Fazenda fazenda)
        {
            Id = fazenda.Id;
            IdEtapa = fazenda.Etapa.Id;
            LocalizacaoFazenda = fazenda.LocalizacaoFazenda != null ? new LocalizacaoFazendaDto(fazenda.LocalizacaoFazenda) : null;
            DadosFazenda = fazenda.DadosFazenda != null ? new DadosFazendaDto(fazenda.DadosFazenda) : null;
            Demarcacao = fazenda.Demarcacao;
            Ativa = fazenda.Ativa;
            Concluida = fazenda.Concluida;
        }
    }
}
