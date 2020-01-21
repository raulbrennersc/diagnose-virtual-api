using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Entities
{
    public class Lavoura : BaseEntity
    {
        public DadosLavoura DadosLavoura { get; set; }
        public Geometria Demarcacao { get; set; }
        public ICollection<Geometria> Talhoes { get; set; }
        public bool Concluida { get; set; }
    }
}
