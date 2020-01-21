using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Entities
{
    public class LocalizacaoGeo
    {
        public int Id { get; set; }
        public ICollection<Geometria> Geometrias { get; set; }
    }
}
