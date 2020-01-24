using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Entities
{
    public class Lavoura : BaseEntity
    {
        public virtual DadosLavoura DadosLavoura { get; set; }
        public virtual Fazenda Fazenda { get; set; }
        public virtual Geometry Demarcacao { get; set; }
        public virtual bool Concluida { get; set; }
        public virtual ICollection<Talhao> Talhoes { get; set; }
        public virtual int IdFazenda { get; set; }
    }
}
