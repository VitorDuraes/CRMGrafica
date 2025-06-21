using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMGraficaAPI.Data;
using CRMGraficaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMGraficaAPI.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly AppDbContext _context;
        private readonly IClienteService _clienteService; // Inject ClienteService to update segmentation

        public PedidoService(AppDbContext context, IClienteService clienteService)
        {
            _context = context;
            _clienteService = clienteService;
        }

        public async Task<IEnumerable<Pedido>> GetAllPedidosAsync()
        {
            return await _context.Pedidos.ToListAsync();
        }

        public async Task<IEnumerable<Pedido>> GetPedidosByClienteAsync(int clienteId)
        {
            return await _context.Pedidos
                                 .Where(p => p.ClienteId == clienteId)
                                 .ToListAsync();
        }

        public async Task<Pedido> GetPedidoByIdAsync(int id)
        {
            return await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pedido> CreatePedidoAsync(Pedido pedido)
        {
            pedido.DataCriacao = DateTime.UtcNow;
            pedido.Status = StatusPedido.Orcamento; // Default status
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            // After saving the pedido, update the client's segmentation
            await _clienteService.AtualizarSegmentacaoClientePorPedido(pedido.ClienteId);

            return pedido;
        }

        public async Task<bool> UpdatePedidoAsync(int id, Pedido pedido)
        {
            var existingPedido = await _context.Pedidos.FindAsync(id);
            if (existingPedido == null)
            {
                return false;
            }

            existingPedido.ServicoSolicitado = pedido.ServicoSolicitado;
            existingPedido.Descricao = pedido.Descricao;
            existingPedido.Valor = pedido.Valor;
            existingPedido.Status = pedido.Status; // Allow full update, including status
            existingPedido.DataAtualizacao = DateTime.UtcNow;
            // ClienteId generally shouldn't be updated this way, handle separately if needed

            _context.Entry(existingPedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PedidoExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        public async Task<bool> UpdatePedidoStatusAsync(int id, StatusPedido novoStatus)
        {
            var existingPedido = await _context.Pedidos.FindAsync(id);
            if (existingPedido == null)
            {
                return false;
            }

            existingPedido.Status = novoStatus;
            existingPedido.DataAtualizacao = DateTime.UtcNow;
            _context.Entry(existingPedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeletePedidoAsync(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return false;
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            // After deleting the pedido, update the client's segmentation
            // Need to be careful here, as the pedido is gone. Fetch client separately.
            var cliente = await _context.Clientes.Include(c => c.Pedidos).FirstOrDefaultAsync(c => c.Id == pedido.ClienteId);
            if (cliente != null)
            {
                await _clienteService.AtualizarSegmentacaoClientePorPedido(cliente.Id);
            }

            return true;
        }

        private async Task<bool> PedidoExists(int id)
        {
            return await _context.Pedidos.AnyAsync(e => e.Id == id);
        }
    }
}