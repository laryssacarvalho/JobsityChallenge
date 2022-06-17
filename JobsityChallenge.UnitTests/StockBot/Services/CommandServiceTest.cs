using Moq;
using Moq.AutoMock;
using StockBot.Messages;
using StockBot.Models;
using StockBot.Services;
using StockBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsityChallenge.UnitTests.StockBot.Services
{
    public class CommandServiceTest
    {
        private readonly AutoMocker _mocker;
        private readonly CommandService _sut;

        public CommandServiceTest()
        {
            _mocker = new();
            _sut = _mocker.CreateInstance<CommandService>();
        }

        [Fact]
        public async Task ExecuteCommand_ShouldExecuteStockQuoteCommandAndPublishSuccessMessage_WhenStockCommandIsPassed()
        {
            //Arrange
            _mocker.GetMock<IStockService>()
                .Setup(x => x.GetStockQuoteByCode(It.IsAny<string>()))
                .ReturnsAsync(100);

            var command = new CommandModel { Command = "/stock", Value = "test" };
            var expectedMessage = $"{command.Value.ToUpper()} quote is $100.00 per share";

            //Act
            await _sut.ExecuteCommand(command);

            //Assert
            _mocker.GetMock<IStockService>()
                .Verify(x => x.GetStockQuoteByCode(It.Is<string>(x => x == command.Value)), Times.Once);
            _mocker.GetMock<IMessagePublisher>()
                .Verify(x => x.PublishMessageOnQueue(It.IsAny<string>(), It.Is<string>(x => x == expectedMessage)), Times.Once);
        }

        [Fact]
        public async Task ExecuteCommand_ShouldExecuteStockQuoteCommandAndPublishErrorMessage_WhenStockCodeIsInvalid()
        {
            //Arrange
            _mocker.GetMock<IStockService>()
                .Setup(x => x.GetStockQuoteByCode(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Error"));

            var command = new CommandModel { Command = "/stock", Value = "test" };
            var expectedMessage = $"Sorry, I couldn't get {command.Value.ToUpper()} quote. Try another stock code.";

            //Act
            await _sut.ExecuteCommand(command);

            //Assert
            _mocker.GetMock<IStockService>()
                .Verify(x => x.GetStockQuoteByCode(It.Is<string>(x => x == command.Value)), Times.Once);
            _mocker.GetMock<IMessagePublisher>()
                .Verify(x => x.PublishMessageOnQueue(It.IsAny<string>(), It.Is<string>(x => x == expectedMessage)), Times.Once);
        }

        [Fact]
        public async Task ExecuteCommand_ShouldThrowNotImplementedException_WhenCommandIsInvalid()
        {
            //Arrange
            var command = new CommandModel { Command = "invalid", Value = "test" };

            //Act
            Task act() => _sut.ExecuteCommand(command);

            //Assert
            var exception = await Assert.ThrowsAsync<NotImplementedException>(act);
            Assert.Equal("This command does not exist.", exception.Message);
            _mocker.GetMock<IStockService>()
                .Verify(x => x.GetStockQuoteByCode(It.IsAny<string>()), Times.Never);
            _mocker.GetMock<IMessagePublisher>()
                .Verify(x => x.PublishMessageOnQueue(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        }
    }
}
