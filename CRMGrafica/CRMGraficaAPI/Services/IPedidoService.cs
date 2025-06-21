using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMGraficaAPI.Models;

namespace CRMGraficaAPI.Services
{
    public interface IPedidoService
    {
        Task<IEnumerable<Pedido>> GetAllPedidosAsync();
        Task<IEnumerable<Pedido>> GetPedidosByClienteAsync(int clienteId);
        Task<Pedido> GetPedidoByIdAsync(int id);
        Task<Pedido> CreatePedidoAsync(Pedido pedido);
        Task<bool> UpdatePedidoAsync(int id, Pedido pedido);
        Task<bool> UpdatePedidoStatusAsync(int id, StatusPedido novoStatus);
        Task<bool> DeletePedidoAsync(int id);
    }
}