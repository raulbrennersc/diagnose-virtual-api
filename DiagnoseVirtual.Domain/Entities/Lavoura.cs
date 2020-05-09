using NetTopologySuite.Geometries;
using System.Collections.Generic;

namespace DiagnoseVirtual.Domain.Entities
{
    public class Lavoura : BaseEntity
    {
        public virtual EtapaLavoura Etapa { get; set; }
        public virtual DadosLavoura DadosLavoura { get; set; }
        public virtual Fazenda Fazenda { get; set; }
        public virtual Polygon Demarcacao { get; set; }
        public virtual bool Concluida { get; set; }
        public virtual MultiPolygon Talhoes { get; set; }
    }
}
