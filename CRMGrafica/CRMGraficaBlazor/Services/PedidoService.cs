using System.Text.Json;
using CRMGraficaBlazor.Models;

namespace CRMGraficaBlazor.Services
{
    public class PedidoService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public PedidoService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CRMApi");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<Pedido>> GetPedidosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Pedidos");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Pedido>>(json, _jsonOptions) ?? new List<Pedido>();
            }
            catch
            {
                return new List<Pedido>();
            }
        }

        public async Task<Pedido?> GetPedidoAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Pedidos/{id}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Pedido>(json, _jsonOptions);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreatePedidoAsync(CreatePedidoDto pedido)
        {
            try
            {
                var json = JsonSerializer.Serialize(pedido);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Pedidos", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdatePedidoStatusAsync(int id, StatusPedido status)
        {
            try
            {
                var json = JsonSerializer.Serialize(status);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PatchAsync($"api/Pedidos/{id}/Status", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeletePedidoAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Pedidos/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}

