﻿namespace DiagnoseVirtual.Domain.Entities
{
    public class LocalizacaoFazenda : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual Municipio Municipio { get; set; }
        public virtual string Proprietario { get; set; }
        public virtual string Gerente { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string Email { get; set; }
        public virtual string PontoReferencia { get; set; }
        public virtual Fazenda Fazenda { get; set; }
    }
}
