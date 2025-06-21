using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMGraficaAPI.Data;
using CRMGraficaAPI.Dtos;
using CRMGraficaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMGraficaAPI.Services
{
    public class ClienteService : IClienteService
    {
        private readonly AppDbContext _context;
        // Define thresholds for segmentation (example values)
        private const int FREQUENCIA_RECORRENTE = 5; // Example: 5 or more purchases = Recorrente
        private const int DIAS_INATIVO = 90; // Example: No purchase in 90 days = Inativo

        public ClienteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente> GetClienteByIdAsync(int id)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente> CreateClienteAsync(CreateClienteDto createClienteDto)
        {
            // Initial segmentation based on lack of history
            createClienteDto.Tipo = TipoCliente.Novo;
            createClienteDto.FrequenciaCompra = 0; // Initialize frequency
            var cliente = new Cliente
            {
                Nome = createClienteDto.Nome,
                Contato = createClienteDto.Contato,
                Tipo = createClienteDto.Tipo,
                FrequenciaCompra = createClienteDto.FrequenciaCompra
                // Adicione outros campos se houver
            };

            // Adiciona ao banco de dados
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return cliente; // retorna a entidade salva
        }

        public async Task<bool> UpdateClienteAsync(int id, Cliente cliente)
        {
            var existingCliente = await _context.Clientes.FindAsync(id);
            if (existingCliente == null)
            {
                return false;
            }

            existingCliente.Nome = cliente.Nome;
            existingCliente.Contato = cliente.Contato;
            // FrequenciaCompra and Tipo should be updated by specific logic (e.g., SegmentarClientesAsync or when a Pedido is added)
            // existingCliente.FrequenciaCompra = cliente.FrequenciaCompra; 
            // existingCliente.Tipo = cliente.Tipo;

            _context.Entry(existingCliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                // Optionally, trigger segmentation after update if relevant data changed
                // await SegmentarClienteAsync(existingCliente.Id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClienteExists(id))
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

        public async Task<bool> DeleteClienteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return false;
            }

            // Consider business rules: what happens to Pedidos? Cascade delete? Set null? Prevent deletion?
            // For now, just remove the client.
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }

        // Simple segmentation logic based on purchase frequency and last purchase date
        public async Task SegmentarClientesAsync()
        {
            var clientes = await _context.Clientes.Include(c => c.Pedidos).ToListAsync();
            var now = DateTime.UtcNow;

            foreach (var cliente in clientes)
            {
                var pedidosCliente = cliente.Pedidos.OrderByDescending(p => p.DataCriacao).ToList();
                cliente.FrequenciaCompra = pedidosCliente.Count; // Update frequency

                var ultimoPedido = pedidosCliente.FirstOrDefault();

                if (ultimoPedido == null)
                {
                    cliente.Tipo = TipoCliente.Novo; // No orders yet
                }
                else if ((now - ultimoPedido.DataCriacao).TotalDays > DIAS_INATIVO)
                {
                    cliente.Tipo = TipoCliente.Inativo;
                }
                else if (cliente.FrequenciaCompra >= FREQUENCIA_RECORRENTE)
                {
                    cliente.Tipo = TipoCliente.Recorrente;
                }
                else
                {
                    // If not Inactive and not enough purchases to be Recorrente, they remain Novo (or could be another category)
                    // This logic might need refinement based on business rules (e.g., a client is Novo until X purchases OR Y days)
                    // For simplicity, if active but not Recorrente, keep as Novo for now.
                    if (cliente.Tipo != TipoCliente.Recorrente) // Avoid downgrading a Recorrente just because they are below threshold temporarily
                        cliente.Tipo = TipoCliente.Novo;
                }
                _context.Entry(cliente).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        private async Task<bool> ClienteExists(int id)
        {
            return await _context.Clientes.AnyAsync(e => e.Id == id);
        }

        // Helper method to be called potentially when a new order is placed
        public async Task AtualizarSegmentacaoClientePorPedido(int clienteId)
        {
            var cliente = await _context.Clientes.Include(c => c.Pedidos).FirstOrDefaultAsync(c => c.Id == clienteId);
            if (cliente != null)
            {
                var pedidosCliente = cliente.Pedidos.OrderByDescending(p => p.DataCriacao).ToList();
                cliente.FrequenciaCompra = pedidosCliente.Count;
                var ultimoPedido = pedidosCliente.FirstOrDefault(); // Should be the one just added
                var now = DateTime.UtcNow;

                // Re-evaluate status based on new purchase
                if (cliente.FrequenciaCompra >= FREQUENCIA_RECORRENTE)
                {
                    cliente.Tipo = TipoCliente.Recorrente;
                }
                else
                {
                    cliente.Tipo = TipoCliente.Novo; // Became active again or still new
                }
                _context.Entry(cliente).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }



    }
}