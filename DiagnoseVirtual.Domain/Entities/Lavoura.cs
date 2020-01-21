using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Entities
{
    public class Lavoura : BaseEntity
    {
        public DadosLavoura DadosLavoura { get; set; }
        public Geometry Demarcacao { get; set; }
        public ICollection<Geometry> Talhoes { get; set; }
        public bool Concluida { get; set; }
    }
}
