using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Entities
{
    public class LocalizacaoGeo : BaseEntity
    {
        public ICollection<Geometria> Geometrias { get; set; }
    }
}
