namespace DiagnoseVirtual.Domain.Entities
{
    public class DadosFazenda : BaseEntity
    {
        public virtual Cultura Cultura { get; set; }
        public virtual double AreaTotal { get; set; }
        public virtual int QuantidadeLavouras { get; set; }
        public virtual Fazenda Fazenda { get; set; }
    }
}
