using DiagnoseVirtual.Domain.Entities;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class LavouraMinDto
    {
        public int Id { get; set; }
        public int IdFazenda { get; set; }
        public string Nome { get; set; }
        public string NomeFazenda { get; set; }
        public int idEtapa { get; set; }
        public bool Concluida { get; set; }

        public LavouraMinDto(Lavoura lavoura)
        {
            Id = lavoura.Id;
            IdFazenda = lavoura.Fazenda.Id;
            idEtapa = lavoura.Etapa.Id;
            Nome = lavoura.DadosLavoura.Nome;
            NomeFazenda = lavoura.Fazenda.LocalizacaoFazenda.Nome;
            Concluida = lavoura.Concluida;
        }
    }
}
