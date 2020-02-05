using System;
using System.Collections.Generic;

namespace DiagnoseVirtual.Domain.Entities
{
    public class Monitoramento : BaseEntity
    {
        public Fazenda Fazenda { get; set; }
        public DateTime DataMonitoramento { get; set; }
        public IList<ProblemaMonitoramento> Problemas { get; set; }
        public IList<UploadMonitoramento> Uploads { get; set; }
    }
}
