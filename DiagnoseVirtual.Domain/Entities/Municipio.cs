namespace DiagnoseVirtual.Domain.Entities
{
    public class Municipio : BaseEntity
    {
        public int CodigoIbge { get; set; }
        public virtual string Nome { get; set; }
        public virtual Estado Estado { get; set; }
    }
}
