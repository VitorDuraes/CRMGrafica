using System.Text.Json;
using CRMGraficaBlazor.DTO;

namespace CRMGraficaBlazor.Services
{
    public class IAService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public IAService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CRMApi");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<PrevisaoVendaDto>> PreverVendasAsync(PrevisaoRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                Console.WriteLine($"Payload enviado: {json}"); // Log do JSON enviado

                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/IA/PreverVendas", content);

                response.EnsureSuccessStatusCode();

                var resultJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<PrevisaoVendaDto>>(resultJson, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao chamar PreverVendasAsync: {ex.Message}");
                throw; // Pode escolher lançar de novo ou retornar lista vazia, conforme quiser
            }
        }






    }


}

