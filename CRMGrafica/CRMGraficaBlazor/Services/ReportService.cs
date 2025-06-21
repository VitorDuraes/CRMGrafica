using System.Text.Json;

namespace CRMGraficaBlazor.Services
{
    public class ReportService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ReportService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CRMApi");
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<object>> GetClientesMaisValiososAsync(int topN = 10)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Reports/ClientesMaisValiosos?topN={topN}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<object>>(json, _jsonOptions) ?? new List<object>();
            }
            catch
            {
                return new List<object>();
            }
        }

        public async Task<List<object>> GetReceitaMensalHistoricaAsync(int ultimosMeses = 12)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Reports/ReceitaMensalHistorica?ultimosMeses={ultimosMeses}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<object>>(json, _jsonOptions) ?? new List<object>();
            }
            catch
            {
                return new List<object>();
            }
        }

        public async Task<byte[]?> ExportarClientesMaisValiososAsync(int topN = 10)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Reports/ExportarClientesMaisValiosos?topN={topN}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}

