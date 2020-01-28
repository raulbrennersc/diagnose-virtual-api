namespace DiagnoseVirtual.Domain.Entities
{
    public class DadosLavoura : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string MesAnoPlantio { get; set; }
        public virtual string Cultivar { get; set; }
        public virtual int NumeroPlantas { get; set; }
        public virtual double EspacamentoVertical { get; set; }
        public virtual double EspacamentoHorizontal { get; set; }
        public virtual string Observacoes { get; set; }
        public virtual Lavoura Lavoura { get; set; }

    }
}
