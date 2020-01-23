using DiagnoseVirtual.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class DadosLavouraDto
    {
        public string Nome { get; set; }
        public string MesAnoPlantio { get; set; }
        public string Cultivar { get; set; }
        public int NumeroPlantas { get; set; }
        public double EspacamentoVertical { get; set; }
        public double EspacamentoHorizontal { get; set; }
        public string Observacoes { get; set; }

        public DadosLavouraDto() { }
        public DadosLavouraDto(DadosLavoura dados)
        {
            Nome = dados.Nome;
            MesAnoPlantio = dados.MesAnoPlantio;
            Cultivar = dados.Cultivar;
            NumeroPlantas = dados.NumeroPlantas;
            EspacamentoVertical = dados.EspacamentoVertical;
            EspacamentoHorizontal = dados.EspacamentoHorizontal;
            Observacoes = dados.Observacoes;
        }
    }
}
