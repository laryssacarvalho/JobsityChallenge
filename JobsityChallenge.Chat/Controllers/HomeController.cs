﻿using JobsityChallenge.Chat.Entities;
using JobsityChallenge.Chat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobsityChallenge.Chat.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<UserEntity> _userManager;

    public HomeController(ILogger<HomeController> logger, UserManager<UserEntity> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        
        ViewBag.UserFullName = $"{currentUser.FirstName} {currentUser.LastName}";
        ViewBag.UserId = currentUser.Id;

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}