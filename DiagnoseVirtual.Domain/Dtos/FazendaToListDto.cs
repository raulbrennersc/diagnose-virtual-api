using DiagnoseVirtual.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class FazendaToListDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public FazendaToListDto(Fazenda fazenda)
        {
            Id = fazenda.Id;
            Nome = fazenda.LocalizacaoFazenda.Nome;
        }
    }
}
