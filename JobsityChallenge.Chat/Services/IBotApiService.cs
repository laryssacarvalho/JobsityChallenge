namespace JobsityChallenge.Chat.Services;

public interface IBotApiService
{
    public Task ExecuteCommand(string commandMessage);
}
