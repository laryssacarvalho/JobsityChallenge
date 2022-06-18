using JobsityChallenge.Chat.Entities;
using JobsityChallenge.Chat.Models;
using JobsityChallenge.Chat.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace JobsityChallenge.Chat.Hubs;

public class ChatHub : Hub
{
    public IMessageRepository _messageRepository { get; set; }
    public ChatHub(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    public async Task SendMessage(string userId, string username, string text)
    {
        var message = new MessageEntity(userId, text);
        await _messageRepository.InsertMessage(message);
        await _messageRepository.Save();
        await Clients.All.SendAsync("ReceiveMessage", username, text);
    }
}
