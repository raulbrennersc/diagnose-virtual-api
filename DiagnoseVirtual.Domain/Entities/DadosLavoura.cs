using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Entities
{
    public class DadosLavoura : BaseEntity
    {
        public string Nome { get; set; }
        public string MesAnoPlantio { get; set; }
        public string Cultivar { get; set; }
        public int NumeroPlantas { get; set; }
        public double EspacamentoVertical { get; set; }
        public double EspacamentoHorizontal { get; set; }
        public string Observacoes { get; set; }

    }
}
