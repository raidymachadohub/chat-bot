using System.Text;
using Chat.Bot.Domain.Constants;
using Chat.Bot.Domain.Model;
using Chat.Bot.Infrastructure.Interfaces.Facade;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chat.Bot.Infrastructure.Facade
{
    public class RabbitMQFacade : IRabbitMQFacade
    {
        private readonly ConnectionFactory _connectionFactory;

        public RabbitMQFacade()
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672
            };
        }

        public async Task SendMessageAsync(Stock stock)
        {
            var message = JsonConvert.SerializeObject(stock);

            using var conn = _connectionFactory.CreateConnection();
            using var channel = conn.CreateModel();

            channel.QueueDeclare(queue: RabbitMQConstant.QueueNameBot,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: RabbitMQConstant.QueueNameBot,
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(message));
        }

        public async Task<Stock> ReceiveMessageAsync()
        {
            using var conn = _connectionFactory.CreateConnection();
            using var channel = conn.CreateModel();

            channel.QueueDeclare(queue: RabbitMQConstant.QueueNameBot,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var result = channel.BasicGet(queue: RabbitMQConstant.QueueNameBot, autoAck: true);
            if (result == null)
                return null;
            
            return JsonConvert.DeserializeObject<Stock>(Encoding.UTF8.GetString(result.Body.ToArray()));
        }
    }
}