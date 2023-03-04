using Autofac.Extras.FakeItEasy;
using Chat.Bot.Domain.Model;
using Chat.Bot.Infrastructure.Facade.Interfaces;
using Chat.Bot.Service.Services;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Chat.Bot.Tests.Services
{
    public class StockServiceTests
    {
        [Fact]
        public async Task Should_return_empty_when_send_stock_code_invalid()
        {
            using var autoFake = new AutoFake();

            var stockFacade = autoFake.Resolve<IStockFacade>();
            A.CallTo(() => stockFacade.GetStockAsync(A<string>.Ignored));

            var service = autoFake.Resolve<StockService>();

            var result = await service.GetValueStockAsync("error");

            result.Should().BeEquivalentTo(new Stock());
        }

        [Fact]
        public async Task Should_return_stock_when_send_stock_code_not_Invalid()
        {
            var stock = @"Symbol,Date,Time,Open,High,Low,Close,Volume
AAPL.US,2023-03-02,22:00:03,144,144,144,144,144";
            
            var expected = new Stock()
            {
                Symbol = "AAPL.US",
                Date = Convert.ToDateTime("2023-03-02"),
                Time = "22:00:03",
                Open = 144,
                High = 144,
                Low = 144,
                Close = 144,
                Volume = 144
            };
            
            using var autoFake = new AutoFake();

            var stockFacade = autoFake.Resolve<IStockFacade>();
            A.CallTo(() => stockFacade.GetStockAsync(A<string>.Ignored)).Returns(stock);

            var service = autoFake.Resolve<StockService>();

            var result = await service.GetValueStockAsync("AAPL.US");

            result.Should().BeEquivalentTo(expected);
        }
    }
}