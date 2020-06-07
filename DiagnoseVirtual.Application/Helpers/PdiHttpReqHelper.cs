using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnoseVirtual.Application.Helpers
{
    public static class PdiHttpReqHelper
    {
        public static dynamic PdiInsertReq(Geometry geo)
        {
            return new dynamic[]
            {
                new {
                    layer = "fazenda",
                    pw = "pdi2020",
                    usr = "app",
                    geometria = new
                    {
                        type = "Polygon",
                        coordinates = new double[][][]
                        {
                            geo.Coordinates.Select(c => new double[]{c.X, c.Y}).ToArray()
                        }
                    }
                }
            };
        }

        public static dynamic PdiQueryReq(string cod)
        {
            return new dynamic[]
            {
                new {
                    layer = "fazenda",
                    pw = "pdi2020",
                    usr = "app",
                    cod = cod,
                }
            };
        }
    }
}
