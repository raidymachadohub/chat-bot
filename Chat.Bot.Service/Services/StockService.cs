using Chat.Bot.Domain.Model;
using Chat.Bot.Infrastructure.Facade.Interfaces;
using Chat.Bot.Service.Services.Interfaces;

namespace Chat.Bot.Service.Services
{
    public class StockService : IStockService
    {
        private readonly IStockFacade _stockFacade;

        public StockService(IStockFacade stockFacade)
        {
            _stockFacade = stockFacade;
        }
        
        public async Task<Stock> GetValueStockAsync(string stockCode)
        {
            var stocks = await _stockFacade.GetStockAsync(stockCode);

            if (stocks.Contains("N/D") || string.IsNullOrEmpty(stocks))
                return new Stock();
            
            var stock = stocks.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .Skip(1)
                .Select(x => x.Split(','))
                .Select(column => new Stock(symbol: column[0],
                                            date: Convert.ToDateTime(column[1]),
                                            time: column[2],
                                            open: Convert.ToDecimal(column[3].Replace(".", ",")),
                                            high: Convert.ToDecimal(column[4].Replace(".", ",")),
                                            low: Convert.ToDecimal(column[5].Replace(".", ",")),
                                            close: Convert.ToDecimal(column[6].Replace(".", ",")),
                                            volume: Convert.ToInt64(column[7].Replace(".", ","))))
                .First();

            return stock;
        }
    }
}