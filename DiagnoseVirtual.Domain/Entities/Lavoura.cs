using NetTopologySuite.Geometries;
using NetTopologySuite.Features;

namespace DiagnoseVirtual.Domain.Entities
{
    public class Lavoura : BaseEntity
    {
        public virtual EtapaLavoura Etapa { get; set; }
        public virtual DadosLavoura DadosLavoura { get; set; }
        public virtual Fazenda Fazenda { get; set; }
        public virtual Geometry Demarcacao { get; set; }
        public virtual bool Concluida { get; set; }
        public virtual bool Ativa { get; set; }
        public virtual Geometry[] Talhoes { get; set; }
        public virtual string IdPdi { get; set; }
    }
}
