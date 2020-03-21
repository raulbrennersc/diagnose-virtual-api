using DiagnoseVirtual.Domain.Entities;
using System;

namespace DiagnoseVirtual.Domain.Dtos
{
    public class MonitoramentoToListDto
    {
        public int Id { get; set; }
        public string NomeFazenda { get; set; }
        public DateTime DataMonitoramento { get; set; }

        public MonitoramentoToListDto(Monitoramento monitoramento)
        {
            Id = monitoramento.Id;
            NomeFazenda = monitoramento.Fazenda.LocalizacaoFazenda.Nome;
            DataMonitoramento = monitoramento.DataMonitoramento;
        }
    }
}