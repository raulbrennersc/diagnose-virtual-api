using DiagnoseVirtual.Domain.Entities;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class UploadMonitoramentoDto
    {
        public string NomeArquivo { get; set; }
        public string Url { get; set; }

        public UploadMonitoramentoDto(UploadMonitoramento upload)
        {
            NomeArquivo = upload.NomeArquivo;
            Url = upload.Url;
        }
    }
}