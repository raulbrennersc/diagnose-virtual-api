using System;
using System.Collections.Generic;

namespace DiagnoseVirtual.Domain.Entities
{
    public class Monitoramento : BaseEntity
    {
        public virtual Fazenda Fazenda { get; set; }
        public DateTime DataMonitoramento { get; set; }
        public string UrlPdi { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual IList<ProblemaMonitoramento> Problemas { get; set; }
        public virtual IList<UploadMonitoramento> Uploads { get; set; }
    }
}
