using JobsityChallenge.Bot.Services;
using Moq.AutoMock;

namespace JobsityChallenge.UnitTests.JobsityChallenge.Bot.Services;

public class StockServiceTest
{
    private readonly AutoMocker _mocker;
    private readonly StockService _sut;
    public StockServiceTest()
    {
        _mocker = new();
        _sut = _mocker.CreateInstance<StockService>();
    }

    [Fact]
    public async Task GetStockQuoteByCode_ShouldReturnQuote_WhenStockCodeIsValid()
    {
        //Arrange
        //_mocker.GetMock<HttpClient>()
        //    .Setup(x => x.GetStreamAsync(It.IsAny<string>()))
        //    .ReturnsAsync();

        //Act
        var result = await _sut.GetStockQuoteByCode("aapl.us");

        //Assert
        Assert.Equal(123, result);
    }

    //private Stream MockCsvFile()
    //{
    //    var record = new StockCsvModel { Close = 123 };

    //    using (var writer = new StreamWriter("path\\to\\file.csv"))
    //    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
    //    {
    //        csv.WriteRecord(record);
    //    }
    //    return csv;
    //}
}
