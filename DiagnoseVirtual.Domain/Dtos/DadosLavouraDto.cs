using DiagnoseVirtual.Domain.Entities;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class DadosLavouraDto
    {
        public string Nome { get; set; }
        public string Data { get; set; }
        public string Cultivar { get; set; }
        public int NumeroPlantas { get; set; }
        public double EspacamentoVertical { get; set; }
        public double EspacamentoHorizontal { get; set; }
        public string Observacao { get; set; }

        public DadosLavouraDto() { }
        public DadosLavouraDto(DadosLavoura dados)
        {
            Nome = dados.Nome;
            Data = dados.MesAnoPlantio;
            Cultivar = dados.Cultivar;
            NumeroPlantas = dados.NumeroPlantas;
            EspacamentoVertical = dados.EspacamentoVertical;
            EspacamentoHorizontal = dados.EspacamentoHorizontal;
            Observacao = dados.Observacoes;
        }
    }
}
