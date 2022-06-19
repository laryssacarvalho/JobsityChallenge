using StockBot.Models;

namespace StockBot.Services.Interfaces
{
    public interface ICommandService
    {
        public Task ExecuteCommand(CommandModel command);
    }
}
