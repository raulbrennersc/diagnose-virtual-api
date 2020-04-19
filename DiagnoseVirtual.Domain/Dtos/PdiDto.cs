using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class PdiDto
    {
        public string usr { get; set; }
        public string pw { get; set; }
        public string layer { get; set; }
        public double espacx { get; set; }
        public double espacy { get; set; }
        public string plantio  { get; set; }
        public string cod_faz { get; set; }
        public string cod_lav { get; set; }
        public string cod { get; set; }
        public string geometria { get; set; }
    }
}
