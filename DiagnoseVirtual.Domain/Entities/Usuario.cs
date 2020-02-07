using System.Collections.Generic;

namespace DiagnoseVirtual.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Cpf { get; set; }
        public virtual string Email { get; set; }
        public virtual byte[] PasswordHash { get; set; }
        public virtual byte[] PasswordSalt { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual IList<Fazenda> Fazendas { get; set; }
    }
}
