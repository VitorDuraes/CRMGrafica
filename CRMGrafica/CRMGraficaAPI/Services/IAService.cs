using Microsoft.ML;
using Microsoft.ML.Data;
using CRMGraficaAPI.Models;
using CRMGraficaAPI.Dtos;
using CRMGraficaAPI.Services;
using System.Text.Json;

public class IAService : IIAService
{
    private readonly MLContext _mlContext = new MLContext();

    public List<PrevisaoVendaDto> Prever(PrevisaoRequest request)
    {
        if (request.Historico == null || request.Historico.Count < 3)
            throw new ArgumentException("Histórico de vendas precisa ter pelo menos 3 registros para previsão.");
        try
        {
            // 1. Preparar dados
            var dados = request.Historico.Select((valor, index) => new ReceitaMensalData
            {
                MesIndex = index,
                Receita = (float)valor
            }).ToList();

            var dataView = _mlContext.Data.LoadFromEnumerable(dados);

            // 2. Pipeline de regressão
            var pipeline = _mlContext.Transforms.Concatenate("Features", "MesIndex")
                .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "Receita"));

            var model = pipeline.Fit(dataView);

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<ReceitaMensalData, ReceitaMensalPrediction>(model);

            // 3. Prever os próximos meses
            var previsoes = new List<PrevisaoVendaDto>();
            var dataBase = DateTime.Parse(request.DataBase + "-01");

            for (int i = 0; i < request.QuantidadeMeses; i++)
            {
                var index = dados.Count + i;
                var prediction = predictionEngine.Predict(new ReceitaMensalData { MesIndex = index });

                previsoes.Add(new PrevisaoVendaDto
                {
                    AnoMes = dataBase.AddMonths(i).ToString("yyyy-MM"),
                    ValorPrevisto = Math.Round(prediction.Score, 2)
                });
            }
            var json = JsonSerializer.Serialize(request);
            Console.WriteLine($"Payload enviado: {json}");

            return previsoes;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro no Prever: {ex.Message}");
            throw;  // para ver o erro real na chamada
        }
    }

    // Classes internas para ML.NET
    private class ReceitaMensalData
    {
        public float MesIndex { get; set; }
        public float Receita { get; set; }
    }

    private class ReceitaMensalPrediction
    {
        public float Score { get; set; }
    }
}
