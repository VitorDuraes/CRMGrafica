using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMGraficaAPI.Services
{
    public class ClientesMaisValiososReport
    {
        public int ClienteId { get; set; }
        public string NomeCliente { get; set; }
        public decimal VolumeTotalCompras { get; set; }
        public int NumeroPedidos { get; set; }
    }

    public class ServicosMaisSolicitadosReport
    {
        public string Servico { get; set; }
        public int QuantidadeSolicitada { get; set; }
        public decimal ValorTotalGerado { get; set; }
    }

    public interface IReportService
    {
        Task<IEnumerable<ClientesMaisValiososReport>> GetClientesMaisValiososAsync(int topN = 10);
        Task<IEnumerable<ServicosMaisSolicitadosReport>> GetServicosMaisSolicitadosAsync(int topN = 10);
        // Receita estimada por mês requires the IA prediction, maybe belongs in IAService or a combined service?
        // Let's assume for now it uses historical data.
        Task<Dictionary<string, decimal>> GetReceitaMensalHistoricaAsync(int ultimosMeses = 12);

        // Methods for generating PDF/Excel could be added here
        // Task<byte[]> GerarRelatorioClientesPdfAsync(int topN = 10);
        // Task<byte[]> GerarRelatorioServicosExcelAsync(int topN = 10);
    }
}