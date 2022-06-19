namespace JobsityChallenge.Bot.Services.Interfaces;

public interface IStockService
{
    public Task<float> GetStockQuoteByCode(string stockCode);
}
