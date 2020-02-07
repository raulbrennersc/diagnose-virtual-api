using System;
using System.Collections.Generic;
using System.Linq;
using DiagnoseVirtual.Domain.Entities;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class MonitoramentoDetailDto
    {
        public int Id { get; set; }
        public string NomeFazenda { get; set; }
        public DateTime DataMonitoramento { get; set; }
        public List<ProblemaMonitoramentoDto> Problemas { get; set; }
        public List<UploadMonitoramentoDto> Uploads { get; set; }
        public GeometriaDto DemarcacaoFazenda { get; set; }
        public List<GeometriaDto> DemarcacaoLavouras { get; set; }
        public List<GeometriaDto> DemarcacaoTalhoes { get; set; }

        public MonitoramentoDetailDto(Monitoramento monitoramento)
        {
            var talhoes = new List<GeometriaDto>();
            foreach (var lavoura in monitoramento.Fazenda.Lavouras)
            {
                talhoes.AddRange(lavoura.Talhoes.Select(t => new GeometriaDto(t.Geometria)));
            }
            
            Id = monitoramento.Id;
            NomeFazenda = monitoramento.Fazenda.LocalizacaoFazenda.Nome;
            DataMonitoramento = monitoramento.DataMonitoramento;
            Problemas = monitoramento.Problemas.Select(p => new ProblemaMonitoramentoDto(p)).ToList();
            Uploads = monitoramento.Uploads.Select(u => new UploadMonitoramentoDto(u)).ToList();
            DemarcacaoFazenda = new GeometriaDto(monitoramento.Fazenda.Demarcacao);
            DemarcacaoLavouras = monitoramento.Fazenda.Lavouras.Select(l => new GeometriaDto(l.Demarcacao)).ToList();
            DemarcacaoTalhoes = talhoes;
        }
    }
}