using JobsityChallenge.Chat.Entities;
using JobsityChallenge.Chat.Repositories;
using JobsityChallenge.Chat.Services;
using Microsoft.AspNetCore.SignalR;

namespace JobsityChallenge.Chat.Hubs;

public class ChatHub : Hub
{
    private readonly IRepository<MessageEntity, int> _messageRepository;
    private readonly IRepository<UserEntity, string> _userRepository;
    private readonly IBotApiService _botService;

    public ChatHub(IRepository<MessageEntity, int> messageRepository, IRepository<UserEntity, string> userRepository, IBotApiService botService)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _botService = botService;
    }
    public async Task SendMessage(string text, string? userId = null)
    {
        var fullName = await GetUserFullName(userId);

        if (IsBotCommand(text))
            await HandleBotCommand(text);
        else
        {
            await SaveMessage(userId, text);

            await Clients.All.SendAsync("ReceiveMessage", $"{fullName}", text);
        }
    }    
    private async Task<string> GetUserFullName(string? userId)
    {
        var fullName = "BOT";

        if (userId is not null)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            fullName = $"{user.FirstName} {user.LastName}";
        }
        return fullName;
    }
    private bool IsBotCommand(string text) => text.StartsWith("/");

    private async Task HandleBotCommand(string text)
    {
        try
        {
            string command = text.Split('=').FirstOrDefault();
            string value = text.Split('=').LastOrDefault();
            await _botService.SendCommandRequest(command, value);
        }
        catch
        {
            await Clients.All.SendAsync("ReceiveMessage", $"BOT", "Sorry, something went wrong!");
        }
    }
    private async Task SaveMessage(string? userId, string text)
    {
        var message = new MessageEntity(userId, text);

        await _messageRepository.AddAsync(message);
        await _messageRepository.Save();
    }
}
