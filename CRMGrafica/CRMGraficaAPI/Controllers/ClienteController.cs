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
    /// Controller responsável pelo gerenciamento de clientes (CRUD e segmentação).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Retorna todos os clientes cadastrados no sistema.
        /// </summary>
        /// <returns>Lista de clientes.</returns>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            var clientes = await _clienteService.GetAllClientesAsync();
            return Ok(clientes);
        }
        /// <summary>
        /// Retorna os dados de um cliente específico com base no ID.
        /// </summary>
        /// <param name="id">ID do cliente.</param>
        /// <returns>Objeto do cliente ou 404 se não encontrado.</returns>

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _clienteService.GetClienteByIdAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }

        /// <summary>
        /// Cria um novo cliente com os dados fornecidos.
        /// </summary>
        /// <param name="cliente">Objeto cliente com os dados de entrada.</param>
        /// <returns>Cliente criado com status 201 ou erro de validação.</returns>

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(CreateClienteDto createClienteDto) // Ideally use a CreateClienteDto
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCliente = await _clienteService.CreateClienteAsync(createClienteDto);
            return CreatedAtAction(nameof(GetCliente), new { id = createdCliente.Id }, createdCliente);
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        /// <param name="id">ID do cliente.</param>
        /// <param name="cliente">Dados atualizados do cliente.</param>
        /// <returns>Status 204 em caso de sucesso, 400 se o ID não coincidir ou 404 se não encontrado.</returns>

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente) // Ideally use an UpdateClienteDto
        {
            if (id != cliente.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _clienteService.UpdateClienteAsync(id, cliente);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Remove um cliente com base no ID.
        /// </summary>
        /// <param name="id">ID do cliente a ser removido.</param>
        /// <returns>Status 204 em caso de sucesso ou 404 se não encontrado.</returns>


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var deleted = await _clienteService.DeleteClienteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Executa a segmentação dos clientes com base em regras específicas definidas no serviço.
        /// </summary>
        /// <returns>Mensagem de sucesso ao concluir a segmentação.</returns>

        [HttpPost("Segmentar")]
        public async Task<IActionResult> SegmentarClientes()
        {
            await _clienteService.SegmentarClientesAsync();
            return Ok("Segmentação de clientes concluída.");
        }
    }
}