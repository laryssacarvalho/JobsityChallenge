namespace StockBot.Services.Interfaces
{
    public interface IStockService
    {
        public Task<float> GetStockQuoteByCode(string stockCode);
    }
}
