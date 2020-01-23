using DiagnoseVirtual.Domain.Dtos;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Entities
{
    public class Fazenda : BaseEntity
    {
        public Usuario Usuario { get; set; }
        public LocalizacaoFazenda LocalizacaoFazenda { get; set; }
        public DadosFazenda DadosFazenda { get; set; }
        public Geometry Demarcacao { get; set; }
        public bool Concluida { get; set; }
        public bool Ativa { get; set; }
        public ICollection<Lavoura> Lavouras { get; set; }
        public int IdUsuario { get; set; }
    }
}
