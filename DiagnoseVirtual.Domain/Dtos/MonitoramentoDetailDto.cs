using DiagnoseVirtual.Domain.Entities;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class MonitoramentoDetailDto
    {
        public int Id { get; set; }
        public int IdFazenda { get; set; }
        public string NomeFazenda { get; set; }
        public string UrlPdi { get; set; }
        public DateTime DataMonitoramento { get; set; }
        public List<ProblemaMonitoramentoDto> Problemas { get; set; }
        public List<UploadMonitoramentoDto> Uploads { get; set; }
        public Geometry DemarcacaoFazenda { get; set; }
        public List<Geometry> DemarcacaoLavouras { get; set; }
        public List<Geometry> DemarcacaoTalhoes { get; set; }

        public MonitoramentoDetailDto(Monitoramento monitoramento)
        {
            Id = monitoramento.Id;
            IdFazenda = monitoramento.Fazenda.Id;
            NomeFazenda = monitoramento.Fazenda.LocalizacaoFazenda.Nome;
            DataMonitoramento = monitoramento.DataMonitoramento;
            Problemas = monitoramento.Problemas.Select(p => new ProblemaMonitoramentoDto(p)).ToList();
            Uploads = monitoramento.Uploads.Select(u => new UploadMonitoramentoDto(u)).ToList();
            DemarcacaoFazenda = monitoramento.Fazenda.Demarcacao;
            DemarcacaoLavouras = monitoramento.Fazenda.Lavouras.Select(l => l.Demarcacao).ToList();
            DemarcacaoTalhoes = new List<Geometry>();
            var listasTalhoes = monitoramento.Fazenda.Lavouras.Select(l => l.Talhoes).ToList();
            foreach (var lista in listasTalhoes)
            {
                foreach (var t in lista)
                {
                    DemarcacaoTalhoes.Add(t);
                }
            }
            UrlPdi = monitoramento.UrlPdi;
        }
    }
}