namespace DiagnoseVirtual.Domain.Entities
{
    public class UploadMonitoramento : BaseEntity
    {
        public virtual Monitoramento Monitoramento { get; set; }
        public string NomeArquivo { get; set; }
        public byte[] Arquivo { get; set; }
    }
}
