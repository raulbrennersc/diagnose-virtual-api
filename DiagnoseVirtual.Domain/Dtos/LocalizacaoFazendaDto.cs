using DiagnoseVirtual.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class LocalizacaoFazendaDto
    {
        public string Nome { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Proprietario { get; set; }
        public string Gerente { get; set; }
        public string Contato { get; set; }
        public string PontoReferencia { get; set; }

        public LocalizacaoFazendaDto() { }
        public LocalizacaoFazendaDto(LocalizacaoFazenda localizacao)
        {
            Nome = localizacao.Nome;
            Estado = localizacao.Estado;
            Municipio = localizacao.Municipio;
            Proprietario = localizacao.Proprietario;
            Gerente = localizacao.Gerente;
            Contato = localizacao.Contato;
            PontoReferencia = localizacao.PontoReferencia;
        }
    }
}
