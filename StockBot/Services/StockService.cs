using CsvHelper;
using StockBot.Models;
using StockBot.Services.Interfaces;
using System.Globalization;

namespace StockBot.Services
{
    public class StockService : IStockService
    {
        private readonly HttpClient _client;
        private readonly string _endpoint = "https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv";
        public StockService(HttpClient client)
        {
            _client = client;

        }

        public async Task<float> GetStockQuoteByCode(string stockCode)
        {
            var response = await _client.GetStreamAsync(string.Format(_endpoint, stockCode));
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
}
