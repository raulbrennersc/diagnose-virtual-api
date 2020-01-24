namespace DiagnoseVirtual.Domain.Entities
{
    public class LocalizacaoFazenda : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Estado { get; set; }
        public virtual string Municipio { get; set; }
        public virtual string Proprietario { get; set; }
        public virtual string Gerente { get; set; }
        public virtual string Contato { get; set; }
        public virtual string PontoReferencia { get; set; }
        public virtual Fazenda Fazenda { get; set; }
        public virtual int IdFazenda { get; set; }
    }
}
