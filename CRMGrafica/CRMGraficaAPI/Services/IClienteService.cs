using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMGraficaAPI.Dtos;
using CRMGraficaAPI.Models;

namespace CRMGraficaAPI.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClientesAsync();
        Task<Cliente> GetClienteByIdAsync(int id);
        Task<Cliente> CreateClienteAsync(CreateClienteDto createClienteDto);
        Task<bool> UpdateClienteAsync(int id, Cliente cliente);
        Task<bool> DeleteClienteAsync(int id);
        Task SegmentarClientesAsync();// Método para a segmentação automática
        Task AtualizarSegmentacaoClientePorPedido(int clienteId);
    }
}