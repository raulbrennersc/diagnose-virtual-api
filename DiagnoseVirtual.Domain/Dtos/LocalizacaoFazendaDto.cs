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
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string PontoReferencia { get; set; }

        public LocalizacaoFazendaDto() { }
        public LocalizacaoFazendaDto(LocalizacaoFazenda localizacao)
        {
            Nome = localizacao.Nome;
            IdMunicipio = localizacao.Municipio?.Id ?? 0;
            IdEstado = localizacao.Municipio?.Estado.Id ?? 0;
            Proprietario = localizacao.Proprietario;
            Gerente = localizacao.Gerente;
            Email = localizacao.Email;
            Telefone = localizacao.Telefone;
            PontoReferencia = localizacao.PontoReferencia;
        }
    }
}
