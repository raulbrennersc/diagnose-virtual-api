using System.Collections.Generic;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class DemarcacaoDto
    {
        public List<GeometriaDto> Geometrias { get; set; }
        public DemarcacaoDto() { }
    }
}
