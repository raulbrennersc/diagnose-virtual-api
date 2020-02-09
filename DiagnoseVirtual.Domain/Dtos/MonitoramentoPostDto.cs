using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class MonitoramentoPostDto
    {
        public int IdFazenda { get; set; }
        public List<ProblemaMonitoramentoDto> Problemas { get; set; }
        public List<IFormFile> Uploads { get; set; }
    }
}