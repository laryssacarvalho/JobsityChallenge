using StockBot.Models;
using StockBot.Services.Interfaces;

namespace StockBot.Services
{
    public class CommandService : ICommandService
    {
        private IStockService _stockService { get; set; }
        public CommandService(IStockService stockService)
        {
            _stockService = stockService;
        }
        public async Task ExecuteCommand(CommandModel command)
        {
            switch (command.Command) 
            {
                case ("/stock"):
                    await GetStockQuoteCommand(command.Value);
                    break;
                default:
                    throw new NotImplementedException("This command does not exist.");
                    break;
            }
        }
        private async Task GetStockQuoteCommand(string stockCode)
        {
            var stockQuote = await _stockService.GetStockQuoteByCode(stockCode);
            //TODO: publish message on queue
        }
    }
}
