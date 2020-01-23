﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Entities
{
    public class LocalizacaoFazenda : BaseEntity
    {
        public string Nome { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Proprietario { get; set; }
        public string Gerente { get; set; }
        public string Contato { get; set; }
        public string PontoReferencia { get; set; }
        public Fazenda Fazenda { get; set; }
        public int IdFazenda { get; set; }
    }
}