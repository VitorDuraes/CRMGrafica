using System.Text.Json;
using CRMGraficaBlazor.Models;

namespace CRMGraficaBlazor.Services
{
    public class ClienteService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ClienteService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CRMApi");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<Cliente>> GetClientesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Clientes");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Cliente>>(json, _jsonOptions) ?? new List<Cliente>();
            }
            catch
            {
                return new List<Cliente>();
            }
        }

        public async Task<Cliente?> GetClienteAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Clientes/{id}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Cliente>(json, _jsonOptions);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateClienteAsync(CreateClienteDto cliente)
        {
            try
            {
                var json = JsonSerializer.Serialize(cliente);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Clientes", content);

                if (!response.IsSuccessStatusCode)
                {
                    var erro = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Erro na criação do cliente: " + erro);
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exceção ao criar cliente: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateClienteAsync(int id, Cliente cliente)
        {
            try
            {
                var json = JsonSerializer.Serialize(cliente);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/Clientes/{id}", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteClienteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Clientes/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}

