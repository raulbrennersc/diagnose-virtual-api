using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class PdiDto
    {
        public string usr { get; set; }
        public string pw { get; set; }
        public string layer { get; set; }
        public string cod { get; set; }
        public double espacx { get; set; }
        public double espacy { get; set; }
        public Geometry geometria { get; set; }

        public PdiDto(Geometry geometria, string layer)
        {
            usr = "app";
            pw = "pdi2020";
            this.layer = layer;
            this.geometria = geometria;
        }
    }
}
