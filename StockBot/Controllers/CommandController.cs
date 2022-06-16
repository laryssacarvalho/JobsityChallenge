using Microsoft.AspNetCore.Mvc;
using StockBot.Models;
using StockBot.Services;
using StockBot.Services.Interfaces;

namespace StockBot.Controllers;

[ApiController]
[Route("[controller]")]
public class CommandController : ControllerBase
{
    private readonly ILogger<CommandController> _logger;
    private readonly ICommandService _commandService;
    public CommandController(ILogger<CommandController> logger, ICommandService commandService)
    {
        _logger = logger;
        _commandService = commandService;
    }

    [HttpPost(Name = "ExecuteCommand")]
    public async Task<IActionResult> Execute(CommandModel command)
    {
        try
        {
            if (!command.IsValid())
                return BadRequest("Invalid command");

            await _commandService.ExecuteCommand(command);

            return Ok();

        } catch(Exception e)
        {
            _logger.LogError(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occured");
        }

    }
}