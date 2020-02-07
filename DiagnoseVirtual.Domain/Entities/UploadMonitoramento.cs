using System;

namespace DiagnoseVirtual.Domain.Entities
{
    public class UploadMonitoramento : BaseEntity
    {
        public virtual Monitoramento Monitoramento { get; set; }
        public string NomeArquivo { get; set; }
        public string Url { get; set; }
    }
}
