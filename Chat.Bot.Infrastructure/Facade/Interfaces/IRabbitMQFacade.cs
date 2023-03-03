using Chat.Bot.Domain.Model;

namespace Chat.Bot.Infrastructure.Interfaces.Facade
{
    public interface IRabbitMQFacade
    {
    Task SendMessageAsync(Stock stock);
    Task<Stock> ReceiveMessageAsync();
    }
}

