using JobsityChallenge.Bot;
using JobsityChallenge.Bot.Controllers;
using JobsityChallenge.Bot.Models;
using JobsityChallenge.Bot.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using System.Net;

namespace JobsityChallenge.UnitTests.JobsityChallenge.Bot.Controllers;

public class CommandControllerTest
{
    private readonly AutoMocker _mocker;
    private readonly CommandController _sut;
    public CommandControllerTest()
    {
        _mocker = new();
        _sut = _mocker.CreateInstance<CommandController>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Execute_ShouldReturnBadRequest_WhenCommandIsEmptyOrNull(string command)
    {
        //Arrange
        var commandRequest = new CommandModel { Command = command };

        //Act
        var result = await _sut.Execute(commandRequest) as ObjectResult;

        //Assert
        Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal("The field 'command' is required", result.Value);
    }

    [Fact]
    public async Task Execute_ShouldReturnBadRequest_WhenCommandIsInvalid()
    {
        //Arrange
        var commandRequest = new CommandModel { Command = "invalid" };

        //Act
        var result = await _sut.Execute(commandRequest) as ObjectResult;

        //Assert
        Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal("Invalid command", result.Value);
    }

    [Fact]
    public async Task Execute_ShouldReturnAccepted_WhenCommandIsValid()
    {
        //Arrange
        var commandRequest = new CommandModel { Command = Constants.ValidCommands.First() };

        //Act
        var result = await _sut.Execute(commandRequest) as ObjectResult;

        //Assert
        Assert.Equal((int)HttpStatusCode.Accepted, result.StatusCode);
    }

    [Fact]
    public async Task Execute_ShouldReturnInternalError_WhenExceptionIsThrown()
    {
        //Arrange
        var commandRequest = new CommandModel { Command = Constants.ValidCommands.First() };
        
        _mocker.GetMock<ICommandService>()
            .Setup(x => x.ExecuteCommand(It.IsAny<CommandModel>()))
            .Throws(new Exception("Error"));
        
        //Act
        var result = await _sut.Execute(commandRequest) as ObjectResult;

        //Assert
        Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
        Assert.Equal("An error occured", result.Value);
    }
}
