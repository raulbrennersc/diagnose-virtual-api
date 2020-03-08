using DiagnoseVirtual.Domain.Entities;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class LocalizacaoFazendaDto
    {
        public string Nome { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Proprietario { get; set; }
        public string Gerente { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string PontoReferencia { get; set; }

        public LocalizacaoFazendaDto() { }
        public LocalizacaoFazendaDto(LocalizacaoFazenda localizacao)
        {
            Nome = localizacao.Nome;
            Estado = localizacao.Estado;
            Municipio = localizacao.Municipio;
            Proprietario = localizacao.Proprietario;
            Gerente = localizacao.Gerente;
            Telefone = localizacao.Telefone;
            Email = localizacao.Email;
            PontoReferencia = localizacao.PontoReferencia;
        }
    }
}
