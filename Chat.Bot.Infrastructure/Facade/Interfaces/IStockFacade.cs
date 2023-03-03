namespace Chat.Bot.Infrastructure.Facade.Interfaces
{
    public interface IStockFacade
    {
        Task<string> GetStockAsync(string stockCode);
    }
}