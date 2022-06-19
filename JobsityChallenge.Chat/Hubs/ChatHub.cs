using JobsityChallenge.Chat.Entities;
using JobsityChallenge.Chat.Models;
using JobsityChallenge.Chat.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace JobsityChallenge.Chat.Hubs;

public class ChatHub : Hub
{
    public IRepository<MessageEntity, int> _messageRepository { get; set; }
    public IRepository<UserEntity, string> _userRepository { get; set; }
    private readonly HttpClient _client;
    private readonly string _endpoint = "https://localhost:7154/Command";

    public ChatHub(IRepository<MessageEntity, int> messageRepository, IRepository<UserEntity, string> userRepository, HttpClient client)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _client = client;
    }
    public async Task SendMessage(string text, string userId = null)
    {
        var fullName = "BOT";

        if(userId is not null)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            fullName = $"{user.FirstName} {user.LastName}";
        }
        
        await SaveMessage(userId, text);

        await Clients.All.SendAsync("ReceiveMessage", $"{fullName}", text);
        
        if (IsBotCommand(text))
            await HandleBotCommand(text);
    }
    private async Task SaveMessage(string userId, string text)
    {
        var message = new MessageEntity(userId, text);

        await _messageRepository.AddAsync(message);
        await _messageRepository.Save();
    }
    private async Task HandleBotCommand(string text)
    {
        string command = text.Split('=').FirstOrDefault();
        string value = text.Split('=').LastOrDefault();

        var request = new CommandRequestModel(command, value);
        var response = await _client.PostAsJsonAsync(_endpoint, request);

        if (response.StatusCode != HttpStatusCode.Accepted)
            await Clients.All.SendAsync("ReceiveMessage", $"BOT", "Sorry, something went wrong!");
    }
    private bool IsBotCommand(string text) => text.StartsWith("/");

}
