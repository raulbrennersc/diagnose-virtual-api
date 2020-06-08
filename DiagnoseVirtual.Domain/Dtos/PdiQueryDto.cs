using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class PdiImages
    {
        public string dates { get; set; }
        public List<string> Images { get; set; }
    }
    public class PdiQueryDto
    {
        public List<PdiImages> Imgs { get; set; }

    }
}
