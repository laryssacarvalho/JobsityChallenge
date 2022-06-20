using JobsityChallenge.Chat.Exceptions;
using JobsityChallenge.Chat.Models;
using JobsityChallenge.Chat.Settings;
using Microsoft.Extensions.Options;
using System.Net;

namespace JobsityChallenge.Chat.Services;

public class BotApiService : IBotApiService
{
    private readonly HttpClient _client;
    private readonly string _endpoint;

    public BotApiService(HttpClient client, IOptions<ApplicationSettings> settings)
    {
        _client = client;
        _endpoint = settings.Value.BotApiEndpoint;
    }

    public async Task ExecuteCommand(string commandMessage)
    {
        string command = commandMessage.Split('=').FirstOrDefault();
        string value = commandMessage.Split('=').LastOrDefault();

        var request = new CommandRequestModel(command, value);
        var response = await _client.PostAsJsonAsync(_endpoint, request);

        if (response.StatusCode == HttpStatusCode.BadRequest)
            throw new BotException("Please enter a valid command");
        
        if (response.StatusCode != HttpStatusCode.Accepted)
            throw new BotException("Something went wrong!");
    }
}
