using JobsityChallenge.Bot.Models;

namespace JobsityChallenge.Bot.Services.Interfaces;

public interface ICommandService
{
    public Task ExecuteCommand(CommandModel command);
}
