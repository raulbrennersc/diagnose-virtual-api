using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class DemarcacaoDto
    {
        public List<GeometriaDto> Geometrias { get; set; }
        public DemarcacaoDto() { }
    }
}
