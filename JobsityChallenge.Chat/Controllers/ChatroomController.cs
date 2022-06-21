using JobsityChallenge.Chat.Entities;
using JobsityChallenge.Chat.Models;
using JobsityChallenge.Chat.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace JobsityChallenge.Chat.Controllers;

[Authorize]
public class ChatroomController : Controller
{
    private readonly ILogger<ChatroomController> _logger;
    private readonly UserManager<UserEntity> _userManager;
    private readonly IRepository<MessageEntity, int> _messageRepository;
    private readonly IRepository<ChatroomEntity, int> _chatroomRepository;

    public ChatroomController(ILogger<ChatroomController> logger, 
                              UserManager<UserEntity> userManager, 
                              IRepository<MessageEntity, int> messageRepository,
                              IRepository<ChatroomEntity, int> chatroomRepository)
    {
        _logger = logger;
        _userManager = userManager;
        _messageRepository = messageRepository;
        _chatroomRepository = chatroomRepository;
    }

    public async Task<IActionResult> Index()
    {
        var chatrooms = await _chatroomRepository.FindAsync();
        return View(chatrooms);
    }

    public async Task<IActionResult> Room(int id)
    {
        var chatroom = await _chatroomRepository.GetByIdAsync(id);
        
        if (chatroom is null)
            return NotFound();

        var loggedUser = await _userManager.GetUserAsync(User);

        var messages = await _messageRepository.FindAsync(x => x.ChatroomId == id, x => x.Include(m => m.User), x => x.OrderByDescending(m => m.Date), 50);        

        ViewBag.UserId = loggedUser.Id;
        ViewBag.ChatId = id;
        ViewBag.Messages = messages.OrderBy(m => m.Date).Select(m =>
        {
            var name = m.UserId is null ? "BOT" : $"{m.User.FirstName} {m.User.LastName}";

            return new { Name = name, Text = m.Text, Date = m.Date.ToString("g") };
        });

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }              

    public IActionResult Create()
    {        
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] ChatroomEntity chatroomEntity)
    {
        if (ModelState.IsValid)
        {
            await _chatroomRepository.AddAsync(chatroomEntity);
            await _chatroomRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(chatroomEntity);
    }
}
