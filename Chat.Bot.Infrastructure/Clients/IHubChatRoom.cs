using Chat.Bot.Domain.Model;

namespace Chat.Bot.Infrastructure.Clients
{
    public interface IHubChatRoom
    {
        Task SendHub(Stock stock);
    }
}

