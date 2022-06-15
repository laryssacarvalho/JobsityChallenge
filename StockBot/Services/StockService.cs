using CsvHelper;
using StockBot.Models;
using StockBot.Services.Interfaces;
using System.Globalization;

namespace StockBot.Services
{
    public class StockService : IStockService
    {
        private readonly HttpClient _client;
        public StockService()
        {
            _client = new HttpClient();
        }
        public async Task<float> GetStockQuoteByCode(string stockCode)
        {
            var response = await _client.GetStreamAsync($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv");
            
            return GetStockFromCsv(response);
        }

        private float GetStockFromCsv(Stream csv)
        {
            using var streamReader = new StreamReader(csv);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            csvReader.Read();
            var record = csvReader.GetRecord<StockCsvModel>();
            return record.Close;
        }
    }
}
