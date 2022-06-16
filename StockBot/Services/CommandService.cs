using StockBot.Messages;
using StockBot.Models;
using StockBot.Services.Interfaces;

namespace StockBot.Services
{
    public class CommandService : ICommandService
    {
        private readonly IStockService _stockService;
        private readonly IMessagePublisher _messagePublisher;

        public CommandService(IStockService stockService, IMessagePublisher messagePublisher)
        {
            _stockService = stockService;
            _messagePublisher = messagePublisher;
        }
        public async Task ExecuteCommand(CommandModel command)
        {
            switch (command.Command) 
            {
                case ("/stock"):
                    await ExecuteStockQuoteCommand(command.Value);
                    break;
                default:
                    throw new NotImplementedException("This command does not exist.");
            }
        }
        private async Task ExecuteStockQuoteCommand(string stockCode)
        {
            var stockQuote = await _stockService.GetStockQuoteByCode(stockCode);
            var message = $"{stockCode} quote is ${stockQuote} per share";
            _messagePublisher.PublishMessageOnQueue("stock-queue", message);
        }
    }
}
