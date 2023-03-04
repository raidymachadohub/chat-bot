using Chat.Bot.Infrastructure.Clients;
using Chat.Bot.Infrastructure.Interfaces.Facade;
using Chat.Bot.Service.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Chat.Bot.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IRabbitMQFacade _rabbitMqFacade;
        private readonly IStockService _stockService;
        private readonly IHubChatRoom _hubChatRoom;

        public Worker(ILogger<Worker> logger,
                      IRabbitMQFacade rabbitMqFacade,
                      IStockService stockService,
                      IHubChatRoom hubChatRoom)
        {
            _rabbitMqFacade = rabbitMqFacade;
            _stockService = stockService;
            _logger = logger;
            _hubChatRoom = hubChatRoom;
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
                        await _hubChatRoom.SendHub(stockResponse);
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