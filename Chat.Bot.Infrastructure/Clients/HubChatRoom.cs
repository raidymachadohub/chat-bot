using Chat.Bot.Domain.Config;
using Chat.Bot.Domain.Model;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace Chat.Bot.Infrastructure.Clients
{
    public class HubChatRoom : IHubChatRoom
    {
        private readonly HubConnectionConfig _hubConnectionConfig;

        public HubChatRoom(IOptions<HubConnectionConfig> hubConnectionConfig)
        {
            _hubConnectionConfig = hubConnectionConfig.Value;
        }

        public async Task SendHub(Stock stock)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(_hubConnectionConfig.Url, (opts) =>
                {
                    opts.HttpMessageHandlerFactory = (message) =>
                    {
                        if (message is HttpClientHandler clientHandler)
                            clientHandler.ServerCertificateCustomValidationCallback +=
                                (sender, certificate, chain, sslPolicyErrors) => true;
                        return message;
                    };
                })
                .Build();


            await connection.StartAsync();

            await connection.SendAsync("SendMessage",
                "CHAT-BOT", string.IsNullOrEmpty(stock.Symbol)
                    ? $"{stock.Symbol} not found"
                    : $"{stock.Symbol} quote is ${stock.Open} per share");
        }
    }
}