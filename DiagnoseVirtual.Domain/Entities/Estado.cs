using System.Collections.Generic;

namespace DiagnoseVirtual.Domain.Entities
{
    public class Estado : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Sigla { get; set; }
        public virtual IList<Municipio> Municipios { get; set; }
    }
}
