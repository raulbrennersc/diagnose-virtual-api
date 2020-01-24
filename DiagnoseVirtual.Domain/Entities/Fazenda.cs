using NetTopologySuite.Geometries;
using System.Collections.Generic;

namespace DiagnoseVirtual.Domain.Entities
{
    public class Fazenda : BaseEntity
    {
        public virtual Usuario Usuario { get; set; }
        public virtual LocalizacaoFazenda LocalizacaoFazenda { get; set; }
        public virtual DadosFazenda DadosFazenda { get; set; }
        public virtual Geometry Demarcacao { get; set; }
        public virtual bool Concluida { get; set; }
        public virtual bool Ativa { get; set; }
        public virtual ICollection<Lavoura> Lavouras { get; set; }
        public virtual int IdUsuario { get; set; }
    }
}
