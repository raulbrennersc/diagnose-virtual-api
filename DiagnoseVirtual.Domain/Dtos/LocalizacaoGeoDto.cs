using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class LocalizacaoGeoDto
    {
        public List<GeometriaDto> Geometrias { get; set; }

        public LocalizacaoGeoDto() { }
        public LocalizacaoGeoDto(LocalizacaoGeo localizacao)
        {
            Geometrias = localizacao?.Geometrias.Select(g => new GeometriaDto(g)).ToList();
        }

    }
}
