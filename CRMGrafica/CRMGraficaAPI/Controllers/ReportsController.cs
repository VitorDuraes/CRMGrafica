using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using CRMGraficaAPI.Data;
using CRMGraficaAPI.Models;
using CRMGraficaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMGraficaAPI.Controllers
{
    /// <summary>
    /// Controller responsável pela geração e exportação de relatórios gerenciais.
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]

    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportsController> _logger;
        private readonly AppDbContext _appDbContext;

        public ReportsController(IReportService reportService, ILogger<ReportsController> logger, AppDbContext appDbContext)
        {
            _reportService = reportService;
            _logger = logger;
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Retorna os clientes com maior volume de compras.
        /// </summary>
        /// <param name="topN">Quantidade de clientes a retornar (padrão: 10).</param>
        /// <returns>Lista dos clientes mais valiosos.</returns>

        [HttpGet("ClientesMaisValiosos")]
        public async Task<IActionResult> GetClientesMaisValiosos([FromQuery] int topN = 10)
        {
            try
            {
                var reportData = await _reportService.GetClientesMaisValiososAsync(topN);
                return Ok(reportData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar relatório de clientes mais valiosos.");
                return StatusCode(500, "Erro interno ao gerar relatório.");
            }
        }

        /// <summary>
        /// Retorna os serviços mais frequentemente solicitados.
        /// </summary>
        /// <param name="topN">Quantidade de serviços a retornar (padrão: 10).</param>
        /// <returns>Lista dos serviços mais populares.</returns>


        [HttpGet("ServicosMaisSolicitados")]
        public async Task<IActionResult> GetServicosMaisSolicitados([FromQuery] int topN = 10)
        {
            try
            {
                var reportData = await _reportService.GetServicosMaisSolicitadosAsync(topN);
                return Ok(reportData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar relatório de serviços mais solicitados.");
                return StatusCode(500, "Erro interno ao gerar relatório.");
            }
        }

        /// <summary>
        /// Retorna a receita mensal dos últimos X meses, agrupada por ano e mês.
        /// </summary>
        /// <param name="ultimosMeses">Quantidade de meses para considerar no histórico.</param>
        /// <returns>Lista de objetos com receita total por mês.</returns>


        [HttpGet("ReceitaMensalHistorica")]
        public async Task<List<object>> GetReceitaMensalHistoricaAsync(int ultimosMeses)
        {
            var dataLimite = DateTime.UtcNow.AddMonths(-ultimosMeses).Date;

            var query = await _appDbContext.Pedidos
                .Where(p => p.DataCriacao >= dataLimite &&
                            (p.Status == StatusPedido.Finalizado || p.Status == StatusPedido.Entregue))
                .GroupBy(p => new { p.DataCriacao.Year, p.DataCriacao.Month })
                .Select(g => new
                {
                    Ano = g.Key.Year,
                    Mes = g.Key.Month,
                    ReceitaTotal = g.Sum(e => e.Valor)
                })
                .OrderBy(g => g.Ano)
                .ThenBy(g => g.Mes)
                .ToListAsync();

            // Agora formata a string Ano-Mês já em memória
            var resultadoFormatado = query.Select(q => new
            {
                AnoMes = $"{q.Ano}-{q.Mes:D2}",
                ReceitaTotal = q.ReceitaTotal
            }).ToList<object>();

            return resultadoFormatado;
        }

        /// <summary>
        /// Gera um arquivo Excel com os clientes mais valiosos e retorna o arquivo para download.
        /// </summary>
        /// <param name="topN">Número de clientes a exportar (padrão: 10).</param>
        /// <returns>Arquivo Excel com os dados formatados.</returns>

        [HttpGet("ExportarClientesMaisValiosos")]
        public async Task<IActionResult> ExportarClientesMaisValiosos([FromQuery] int topN = 10)
        {
            var dados = await _reportService.GetClientesMaisValiososAsync(topN);
            var stream = new MemoryStream();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ClientesMaisValiosos");

                // Cabeçalho
                worksheet.Cell(1, 1).Value = "ID Cliente";
                worksheet.Cell(1, 2).Value = "Nome";
                worksheet.Cell(1, 3).Value = "Volume Total Compras";
                worksheet.Cell(1, 4).Value = "Número de Pedidos";

                int row = 2;
                foreach (var item in dados)
                {
                    worksheet.Cell(row, 1).Value = item.ClienteId;
                    worksheet.Cell(row, 2).Value = item.NomeCliente;
                    worksheet.Cell(row, 3).Value = item.VolumeTotalCompras;
                    worksheet.Cell(row, 4).Value = item.NumeroPedidos;
                    row++;
                }

                workbook.SaveAs(stream);
            }

            stream.Position = 0;

            return File(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"ClientesMaisValiosos_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }
    }
}