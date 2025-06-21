using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMGraficaAPI.Dtos;
using CRMGraficaAPI.Models;
using CRMGraficaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRMGraficaAPI.Controllers
{
    /// <summary>
    /// Controller responsável pelo gerenciamento de pedidos (CRUD e consultas específicas).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        /// <summary>
        /// Retorna todos os pedidos cadastrados no sistema.
        /// </summary>
        /// <returns>Lista de pedidos.</returns>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoDto>>> GetPedidos()
        {
            var pedidos = await _pedidoService.GetAllPedidosAsync();
            var pedidosDto = pedidos.Select(p => new PedidoDto
            {
                Id = p.Id,
                ServicoSolicitado = p.ServicoSolicitado,
                Descricao = p.Descricao,
                Valor = p.Valor,
                Status = p.Status,
                DataCriacao = p.DataCriacao,
                DataAtualizacao = p.DataAtualizacao,
                ClienteId = p.ClienteId
            });
            return Ok(pedidosDto);
        }

        /// <summary>
        /// Retorna um pedido específico pelo ID.
        /// </summary>
        /// <param name="id">ID do pedido.</param>
        /// <returns>Objeto do pedido ou 404 se não encontrado.</returns>

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDto>> GetPedido(int id)
        {
            var pedido = await _pedidoService.GetPedidoByIdAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            var pedidoDto = new PedidoDto
            {
                Id = pedido.Id,
                ServicoSolicitado = pedido.ServicoSolicitado,
                Descricao = pedido.Descricao,
                Valor = pedido.Valor,
                Status = pedido.Status,
                DataCriacao = pedido.DataCriacao,
                DataAtualizacao = pedido.DataAtualizacao,
                ClienteId = pedido.ClienteId
            };

            return Ok(pedidoDto);
        }

        /// <summary>
        /// Retorna todos os pedidos associados a um cliente específico.
        /// </summary>
        /// <param name="clienteId">ID do cliente.</param>
        /// <returns>Lista de pedidos do cliente.</returns>

        [HttpGet("Cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidosPorCliente(int clienteId)
        {
            var pedidos = await _pedidoService.GetPedidosByClienteAsync(clienteId);

            return Ok(pedidos);
        }

        /// <summary>
        /// Cria um novo pedido com base nos dados fornecidos.
        /// </summary>
        /// <param name="pedidoDto">Dados para criação do pedido.</param>
        /// <returns>Pedido criado com status 201 ou erro de validação.</returns>

        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido([FromBody] CreatePedidoDto pedidoDto) // <-- Mude o tipo do parâmetro
        {
            // Reative esta validação!
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapeia do DTO para a Entidade
            var pedido = new Pedido
            {
                ServicoSolicitado = pedidoDto.ServicoSolicitado,
                Descricao = pedidoDto.Descricao,
                Valor = pedidoDto.Valor,
                ClienteId = pedidoDto.ClienteId
                // Status, DataCriacao, etc., serão definidos pelo PedidoService
            };

            var createdPedido = await _pedidoService.CreatePedidoAsync(pedido);

            // Busca o pedido completo para retornar (incluindo ID, status, etc.)
            var pedidoCompleto = await _pedidoService.GetPedidoByIdAsync(createdPedido.Id);
            if (pedidoCompleto == null)
            {
                // Log ou tratamento de erro, caso não encontre o pedido recém-criado
                return StatusCode(500, "Erro interno ao recuperar o pedido após a criação.");
            }

            // Retorna 201 Created com a localização e o objeto Pedido completo
            return CreatedAtAction(nameof(GetPedido), new { id = pedidoCompleto.Id }, pedidoCompleto);
        }

        /// <summary>
        /// Atualiza os dados de um pedido existente.
        /// </summary>
        /// <param name="id">ID do pedido a ser atualizado.</param>
        /// <param name="pedido">Objeto do pedido com novos dados.</param>
        /// <returns>Status 204 em caso de sucesso, 400 se o ID não coincidir ou 404 se não encontrado.</returns>

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(int id, Pedido pedido) // Ideally use an UpdatePedidoDto
        {
            if (id != pedido.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _pedidoService.UpdatePedidoAsync(id, pedido);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }


        /// <summary>
        /// Atualiza o status de um pedido específico.
        /// </summary>
        /// <param name="id">ID do pedido.</param>
        /// <param name="novoStatus">Novo status a ser atribuído.</param>
        /// <returns>Status 204 em caso de sucesso ou 404 se não encontrado.</returns>

        [HttpPatch("{id}/Status")]
        public async Task<IActionResult> PatchPedidoStatus(int id, [FromBody] StatusPedido novoStatus) // Receive status from body
        {
            var updated = await _pedidoService.UpdatePedidoStatusAsync(id, novoStatus);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Exclui um pedido pelo ID.
        /// </summary>
        /// <param name="id">ID do pedido.</param>
        /// <returns>Status 204 em caso de sucesso ou 404 se não encontrado.</returns>

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var deleted = await _pedidoService.DeletePedidoAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}