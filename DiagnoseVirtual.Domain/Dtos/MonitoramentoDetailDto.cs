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
        public DateTime DataImagemPdi { get; set; }
        public List<ProblemaMonitoramentoDto> Problemas { get; set; }
        public List<UploadMonitoramentoDto> Uploads { get; set; }
        public Geometry DemarcacaoFazenda { get; set; }
        public List<Geometry> DemarcacoesLavoura { get; set; }
        public List<Geometry> DemarcacoesTalhao { get; set; }

        public MonitoramentoDetailDto(Monitoramento monitoramento)
        {
            Id = monitoramento.Id;
            IdFazenda = monitoramento.Fazenda.Id;
            NomeFazenda = monitoramento.Fazenda.LocalizacaoFazenda.Nome;
            DataMonitoramento = monitoramento.DataMonitoramento;
            Problemas = monitoramento.Problemas.Select(p => new ProblemaMonitoramentoDto(p)).ToList();
            Uploads = monitoramento.Uploads.Select(u => new UploadMonitoramentoDto(u)).ToList();
            DemarcacaoFazenda = monitoramento.Fazenda.Demarcacao;
            DemarcacoesLavoura = monitoramento.Fazenda.Lavouras.Select(l => l.Demarcacao).ToList();
            DemarcacoesTalhao = new List<Geometry>();
            var listasTalhoes = monitoramento.Fazenda.Lavouras.Select(l => l.Talhoes).ToList();
            foreach (var lista in listasTalhoes)
            {
                foreach (var t in lista)
                {
                    DemarcacoesTalhao.Add(t);
                }
            }
            UrlPdi = monitoramento.UrlPdi;
        }

        public MonitoramentoDetailDto(Fazenda fazenda)
        {
            DataImagemPdi = DateTime.Now;
            DemarcacaoFazenda = fazenda.Demarcacao;
            DemarcacoesLavoura = fazenda.Lavouras.Select(l => l.Demarcacao).ToList();
            DemarcacoesTalhao = new List<Geometry>();
            var listasTalhoes = fazenda.Lavouras.Select(l => l.Talhoes).ToList();
            foreach (var lista in listasTalhoes)
            {
                foreach (var t in lista)
                {
                    DemarcacoesTalhao.Add(t);
                }
            }
            UrlPdi = "";
        }
    }
}