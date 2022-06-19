namespace JobsityChallenge.Chat.Services;

public interface IBotApiService
{
    public Task SendCommandRequest(string command, string value = null);
}
