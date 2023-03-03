using Chat.Bot.Domain.Model;

namespace Chat.Bot.Service.Services.Interfaces
{
    public interface IStockService
    {
        Task<Stock> GetValueStockAsync(string stockCode);
    }
}

