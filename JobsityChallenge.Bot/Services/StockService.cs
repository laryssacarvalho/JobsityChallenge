using CsvHelper;
using JobsityChallenge.Bot.Models;
using JobsityChallenge.Bot.Services.Interfaces;
using JobsityChallenge.Bot.Settings;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace JobsityChallenge.Bot.Services;

public class StockService : IStockService
{
    private readonly HttpClient _client;
    private readonly string _baseApiEndpoint;
    private readonly string _csvStockQuotePath = "/?s={0}&f=sd2t2ohlcv&h&e=csv";

    public StockService(HttpClient client, IOptions<ApplicationSettings> settings)
    {
        _client = client;
        _baseApiEndpoint = settings.Value.StockApiEndpoint;
    }

    public async Task<float> GetStockQuoteByCode(string stockCode)
    {
        var response = await _client.GetStreamAsync($"{_baseApiEndpoint}{string.Format(_csvStockQuotePath, stockCode)}");
        return GetStockQuoteFromCsv(response);
    }

    private float GetStockQuoteFromCsv(Stream csv)
    {
        using var streamReader = new StreamReader(csv);
        using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
        csvReader.Read();
        var record = csvReader.GetRecord<StockCsvModel>();
        return record.Close;
    }
}
