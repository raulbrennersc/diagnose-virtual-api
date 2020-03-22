using DiagnoseVirtual.Domain.Entities;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class LocalizacaoFazendaDto
    {
        public string Nome { get; set; }
        public int IdMunicipio { get; set; }
        public int IdEstado { get; set; }
        public string Proprietario { get; set; }
        public string Gerente { get; set; }
        public string Contato { get; set; }
        public string PontoReferencia { get; set; }

        public LocalizacaoFazendaDto() { }
        public LocalizacaoFazendaDto(LocalizacaoFazenda localizacao)
        {
            Nome = localizacao.Nome;
            IdMunicipio = localizacao.Municipio?.Id ?? 0;
            IdEstado = localizacao.Municipio?.Estado.Id ?? 0;
            Proprietario = localizacao.Proprietario;
            Gerente = localizacao.Gerente;
            Contato = localizacao.Contato;
            PontoReferencia = localizacao.PontoReferencia;
        }
    }
}
