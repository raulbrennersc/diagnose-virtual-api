using NetTopologySuite.Geometries;
using System.Linq;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class GeometriaDto
    {
        public string Type { get; set; }
        public double[][] Coordinates { get; set; }


        public GeometriaDto() { }
        public GeometriaDto(Geometry geometria)
        {
            Type = geometria?.OgcGeometryType.ToString();
            Coordinates = geometria?.Coordinates.Select(c => new double[]
            {
                c.X,
                c.Y
            }).ToArray();
        }
    }
}
