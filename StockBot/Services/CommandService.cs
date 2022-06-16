using StockBot.Messages;
using StockBot.Models;
using StockBot.Services.Interfaces;

namespace StockBot.Services
{
    public class CommandService : ICommandService
    {
        private readonly IStockService _stockService;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<CommandService> _logger;
        public CommandService(IStockService stockService, IMessagePublisher messagePublisher, ILogger<CommandService> logger)
        {
            _stockService = stockService;
            _messagePublisher = messagePublisher;
            _logger = logger;
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
            try
            {
                var stockQuote = (await _stockService.GetStockQuoteByCode(stockCode)).ToString("F", System.Globalization.CultureInfo.InvariantCulture);
                var message = $"{stockCode.ToUpper()} quote is ${stockQuote} per share";
                _messagePublisher.PublishMessageOnQueue("stock-queue", message);
                _logger.LogInformation("Stock quote success message sent");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                var message = $"Sorry, I couldn't get {stockCode.ToUpper()} quote. Try another stock code.";
                _messagePublisher.PublishMessageOnQueue("stock-queue", message);
            }
        }
    }
}
