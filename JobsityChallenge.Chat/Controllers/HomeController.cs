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
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<UserEntity> _userManager;
    private readonly IRepository<MessageEntity, int> _messageRepository;

    public HomeController(ILogger<HomeController> logger, UserManager<UserEntity> userManager, IRepository<MessageEntity, int> messageRepository)
    {
        _logger = logger;
        _userManager = userManager;
        _messageRepository = messageRepository;
    }

    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        
        ViewBag.UserId = currentUser.Id;
        var messages = await _messageRepository.GetAllAsync(x => x.Include(m => m.User), x => x.OrderByDescending(m => m.Date), 50);        

        ViewBag.Messages = messages.OrderBy(m => m.Date).Select(m =>
        {
            var name = m.UserId is null ? "BOT" : $"{m.User.FirstName} {m.User.LastName}";

            return new { Name = name, Text = m.Text, Date = m.Date.ToString("g") };
        });

        //ViewBag.Messages = messages.OrderBy(m => m.Date).Select(m => new { Name = $"{m.User.FirstName} {m.User.LastName}", Text = m.Text });

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
