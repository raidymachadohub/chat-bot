using Chat.Bot.Infrastructure.Interfaces.Facade;
using Chat.Bot.Service.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Client;

namespace Chat.Bot.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IRabbitMQFacade _rabbitMqFacade;
        private readonly IStockService _stockService;

        public Worker(ILogger<Worker> logger,
            IRabbitMQFacade rabbitMqFacade,
            IStockService stockService)
        {
            _rabbitMqFacade = rabbitMqFacade;
            _stockService = stockService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var messageResponse = await _rabbitMqFacade.ReceiveMessageAsync();

                    if (messageResponse == null)
                    {
                        _logger.LogTrace("There isn't message");
                        continue;
                    }
                    else
                    {
                        _logger.LogTrace("There is message");

                        var stockResponse = await _stockService.GetValueStockAsync(messageResponse.Symbol);

                        var connection = new HubConnectionBuilder()
                            .WithUrl("https://localhost:7250/ChatHub", (opts) =>
                            {
                                opts.HttpMessageHandlerFactory = (message) =>
                                {
                                    if (message is HttpClientHandler clientHandler)
                                        clientHandler.ServerCertificateCustomValidationCallback +=
                                            (sender, certificate, chain, sslPolicyErrors) => { return true; };
                                    return message;
                                };
                            })
                            .Build();

                        
                        await connection.StartAsync(stoppingToken);
                        
                        await connection.SendAsync("SendMessage",
                            "CHAT-BOT", string.IsNullOrEmpty(stockResponse.Symbol) ? $"{messageResponse.Symbol} not found" 
                                : $"{stockResponse.Symbol} quote is ${stockResponse.Open} per share",
                            cancellationToken: stoppingToken);
                    }
                    await Task.Delay(1000, stoppingToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}