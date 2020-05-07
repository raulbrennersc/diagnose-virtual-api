using DiagnoseVirtual.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class FazendaMinDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int IdEtapa { get; set; }
        public bool Concluida { get; set; }

        public FazendaMinDto(Fazenda fazenda)
        {
            Id = fazenda.Id;
            Nome = fazenda.LocalizacaoFazenda.Nome;
            IdEtapa = fazenda.Etapa.Id;
            Concluida = fazenda.Concluida;
        }
    }
}
