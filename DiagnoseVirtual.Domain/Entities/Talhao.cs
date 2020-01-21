using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Entities
{
    public class Talhao: BaseEntity
    {
        public Geometry Geometry { get; set; }
    }
}
