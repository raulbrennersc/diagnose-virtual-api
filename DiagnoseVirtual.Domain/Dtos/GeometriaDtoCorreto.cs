using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class GeometriaDtoCorreto
    {
        public string Type { get; set; }
        public double[][][] Coordinates { get; set; }


        public GeometriaDtoCorreto() { }
        public GeometriaDtoCorreto(Geometry geometria)
        {
            Type = geometria?.OgcGeometryType.ToString();
            Coordinates = new double[][][]{
                geometria?.Coordinates.Select(c => new double[]
                {
                    c.X,
                    c.Y
                }).ToArray()
            };
        }
    }
}
