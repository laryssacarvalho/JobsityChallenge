using JobsityChallenge.Bot.Messages;
using JobsityChallenge.Bot.Models;
using JobsityChallenge.Bot.Services.Interfaces;
using JobsityChallenge.Bot.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace JobsityChallenge.Bot.Services;

public class CommandService : ICommandService
{
    private readonly IStockService _stockService;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<CommandService> _logger;
    private readonly string _queueName;
    private readonly string _queueHost;

    public CommandService(IStockService stockService, IMessagePublisher messagePublisher, ILogger<CommandService> logger, IOptions<ApplicationSettings> settings)
    {
        _stockService = stockService;
        _messagePublisher = messagePublisher;
        _logger = logger;
        _queueName = settings.Value.StockQueueName;
        _queueHost = settings.Value.RabbitMqHost;
    }
    public async Task ExecuteCommand(CommandModel command)
    {
        switch (command.Command)
        {
            case ("/stock"):
                await ExecuteStockQuoteCommand(command);
                break;
            default:
                throw new NotImplementedException("This command does not exist.");
        }
    }
    private async Task ExecuteStockQuoteCommand(CommandModel command)
    {
        try
        {
            var stockQuote = (await _stockService.GetStockQuoteByCode(command.Value)).ToString("F", System.Globalization.CultureInfo.InvariantCulture);
            var text = $"{command.Value.ToUpper()} quote is ${stockQuote} per share";
            var message = new StockQuoteResponseMessage(text, command.ChatId);
            _messagePublisher.PublishMessageOnQueue(_queueName, _queueHost, message);
            _logger.LogInformation("Stock quote success message sent");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            var text = $"Sorry, I couldn't get {command.Value.ToUpper()} quote. Try another stock code.";
            var message = new StockQuoteResponseMessage(text, command.ChatId);
            _messagePublisher.PublishMessageOnQueue("stock-queue", _queueHost, message);
        }
    }
}
