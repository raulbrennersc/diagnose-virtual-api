using DiagnoseVirtual.Domain.Entities;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class UploadMonitoramentoDto
    {
        public string NomeArquivo { get; set; }
        public byte[] Arquivo { get; set; }

        public UploadMonitoramentoDto(UploadMonitoramento upload)
        {
            NomeArquivo = upload.NomeArquivo;
            Arquivo = upload.Arquivo;
        }
    }
}