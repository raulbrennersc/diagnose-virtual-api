using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class MonitoramentoPostDto
    {
        public int IdFazenda { get; set; }
        public List<ProblemaMonitoramentoDto> Problemas { get; set; }
        public List<IFormFile> Uploads { get; set; }
    }
}