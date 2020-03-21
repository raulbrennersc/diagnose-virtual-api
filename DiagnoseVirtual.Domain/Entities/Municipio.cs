namespace DiagnoseVirtual.Domain.Entities
{
    public class Municipio
    {
        public virtual int Id { get; set; }
        public int CodigoIbge { get; set; }
        public virtual string Nome { get; set; }
        public virtual Estado Estado { get; set; }
    }
}
