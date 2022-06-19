using JobsityChallenge.Bot.Models;
using JobsityChallenge.Bot;

namespace JobsityChallenge.UnitTests.JobsityChallenge.Bot.Models;

public class CommandModelTest
{
    [Fact]
    public void IsValid_ShouldReturnFalse_WhenCommandIsInvalid()
    {
        //Arrange
        var model = new CommandModel { Command = "invalid" };

        //Act
        var result = model.IsValid();

        //Arrange
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenCommandIsValid()
    {
        //Arrange
        var model = new CommandModel { Command = Constants.ValidCommands.First() };

        //Act
        var result = model.IsValid();

        //Arrange
        Assert.True(result);
    }
}
