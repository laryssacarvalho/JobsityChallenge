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
        if (IsBotCommand(text))
            await HandleBotCommand(text);
        else
            await HandleUserMessage(text, userId);
    }    
    private bool IsBotCommand(string text) => text.StartsWith("/");

    private async Task HandleBotCommand(string text)
    {
        try
        {
            await _botService.ExecuteCommand(text);
        }
        catch(Exception ex)
        {
            await Clients.All.SendAsync("ReceiveMessage", "BOT", $"Sorry, unable to process your command. {ex.Message}");
        }
    }
    private async Task HandleUserMessage(string text, string userId)
    {
        var fullName = await GetUserFullName(userId);

        await SaveMessage(userId, text);

        await Clients.All.SendAsync("ReceiveMessage", $"{fullName}", text);
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
    private async Task SaveMessage(string? userId, string text)
    {
        await _messageRepository.AddAsync(new MessageEntity(userId, text));
        await _messageRepository.Save();
    }
}
