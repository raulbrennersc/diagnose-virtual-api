using DiagnoseVirtual.Domain.Entities;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class LavouraMinDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int idEtapa { get; set; }
        public bool Concluida { get; set; }

        public LavouraMinDto(Lavoura lavoura)
        {
            Id = lavoura.Id;
            idEtapa = lavoura.Etapa.Id;
            Nome = lavoura.DadosLavoura.Nome;
            Concluida = lavoura.Concluida;
        }
    }
}
