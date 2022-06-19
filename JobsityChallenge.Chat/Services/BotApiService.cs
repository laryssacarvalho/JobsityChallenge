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

    public async Task SendCommandRequest(string command, string value = null)
    {
        var request = new CommandRequestModel(command, value);
        var response = await _client.PostAsJsonAsync(_endpoint, request);

        if (response.StatusCode != HttpStatusCode.Accepted)
            throw new Exception("Error");
    }
}
