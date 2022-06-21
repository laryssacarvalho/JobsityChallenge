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
    public Task JoinGroup(int chatId)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }
    public async Task SendMessage(string text, int chatId, string? userId = null)
    {
        if (IsBotCommand(text))
            await HandleBotCommand(text, chatId);
        else
            await HandleUserMessage(text, userId, chatId);
    }    
    private bool IsBotCommand(string text) => text.StartsWith("/");

    private async Task HandleBotCommand(string text, int chatId)
    {
        try
        {
            await _botService.ExecuteCommand(text, chatId);
        }
        catch(Exception ex)
        {
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", "BOT", $"Sorry, unable to process your command. {ex.Message}");

            //await Clients.All.SendAsync("ReceiveMessage", "BOT", $"Sorry, unable to process your command. {ex.Message}");
        }
    }
    private async Task HandleUserMessage(string text, string userId, int chatId)
    {
        var fullName = await GetUserFullName(userId);
        
        await SaveMessage(userId, text, chatId);

        //await Clients.All.SendAsync("ReceiveMessage", $"{fullName}", text);
        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", $"{fullName}", text);

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
    private async Task SaveMessage(string? userId, string text, int chatId)
    {
        await _messageRepository.AddAsync(new MessageEntity(userId, text, chatId));
        await _messageRepository.SaveAsync();
    }
}
