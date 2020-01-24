using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Entities
{
    public class Talhao: BaseEntity
    {
        public virtual Geometry Geometria { get; set; }
        public virtual Lavoura Lavoura { get; set; }
        public virtual int IdLavoura { get; set; }
    }
}
