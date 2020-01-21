using DiagnoseVirtual.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class GeometriaDto
    {
        public string Type { get; set; }
        public double[][] Coordinates { get; set; }


        public GeometriaDto() { }
        //public GeometriaDto(Geometria geometria)
        //{
        //    Type = geometria.Geometry.OgcGeometryType.ToString();
        //    Coordinates = geometria.Geometry.Coordinates.Select(c => new double[]
        //    {
        //        c.X,
        //        c.Y
        //    }).ToArray();
        //}
    }
}
