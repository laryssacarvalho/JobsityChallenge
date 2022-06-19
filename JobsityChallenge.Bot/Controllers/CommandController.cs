using Microsoft.AspNetCore.Mvc;
using JobsityChallenge.Bot.Models;
using JobsityChallenge.Bot.Services.Interfaces;

namespace JobsityChallenge.Bot.Controllers;

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
            if (string.IsNullOrEmpty(command.Command))
                return BadRequest("The field 'command' is required");

            if (!command.IsValid())
                return BadRequest("Invalid command");

            await _commandService.ExecuteCommand(command);

            _logger.LogInformation("Command executed!");

            return Accepted();

        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occured");
        }

    }
}