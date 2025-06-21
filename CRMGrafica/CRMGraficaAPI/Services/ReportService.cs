using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMGraficaAPI.Data;
using CRMGraficaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMGraficaAPI.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClientesMaisValiososReport>> GetClientesMaisValiososAsync(int topN = 10)
        {
            // Ensure Pedidos are loaded if not already eager loaded elsewhere
            var clientesComPedidos = await _context.Clientes
                                                .Include(c => c.Pedidos)
                                                .ToListAsync();

            return clientesComPedidos
                .Select(c => new ClientesMaisValiososReport
                {
                    ClienteId = c.Id,
                    NomeCliente = c.Nome,
                    // Consider only completed/paid orders for volume?
                    // For now, sum all orders linked to the client.
                    VolumeTotalCompras = c.Pedidos.Sum(p => p.Valor),
                    NumeroPedidos = c.Pedidos.Count
                })
                .Where(r => r.NumeroPedidos > 0) // Only clients with orders
                .OrderByDescending(r => r.VolumeTotalCompras)
                .Take(topN)
                .ToList(); // Execute the query
        }

        public async Task<IEnumerable<ServicosMaisSolicitadosReport>> GetServicosMaisSolicitadosAsync(int topN = 10)
        {
            // Group pedidos by service type and calculate aggregates
            return await _context.Pedidos
                .Where(p => !string.IsNullOrEmpty(p.ServicoSolicitado)) // Ensure service name exists
                                                                        // Consider filtering by status (e.g., only Aprovado or Finalizado?)
                .GroupBy(p => p.ServicoSolicitado)
                .Select(g => new ServicosMaisSolicitadosReport
                {
                    Servico = g.Key,
                    QuantidadeSolicitada = g.Count(),
                    ValorTotalGerado = g.Sum(p => p.Valor)
                })
                .OrderByDescending(r => r.QuantidadeSolicitada) // Or by ValorTotalGerado
                .Take(topN)
                .ToListAsync();
        }

        public async Task<Dictionary<string, decimal>> GetReceitaMensalHistoricaAsync(int ultimosMeses = 12)
        {
            var dataLimite = DateTime.UtcNow.AddMonths(-ultimosMeses);

            // Group completed/paid orders by month
            var receitaMensal = await _context.Pedidos
                .Where(p => p.DataCriacao >= dataLimite &&
                            (p.Status == StatusPedido.Finalizado || p.Status == StatusPedido.Entregue)) // Consider only completed/paid orders
                .GroupBy(p => new { p.DataCriacao.Year, p.DataCriacao.Month })
                .Select(g => new
                {
                    AnoMes = $"{g.Key.Year}-{g.Key.Month:D2}", // Format YYYY-MM
                    ReceitaTotal = g.Sum(p => p.Valor)
                })
                .OrderBy(r => r.AnoMes)
                .ToDictionaryAsync(r => r.AnoMes, r => r.ReceitaTotal);

            return receitaMensal;
        }
    }
}