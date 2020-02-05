using System;
using NetTopologySuite.Geometries;

namespace DiagnoseVirtual.Domain.Entities
{
    public class ProblemaMonitoramento : BaseEntity
    {
        public virtual Monitoramento Monitoramento { get; set; }
        public Geometry Ponto { get; set; }
        public string Descricao { get; set; }
        public string Recomendacao { get; set; }
    }
}
