using Chat.Bot.Infrastructure.Di;
using Chat.Bot.Service.Di;
using Chat.Bot.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Program
{
    static void Main(string[] args)
    {
        // Setup Host
        var host = CreateDefaultBuilder().Build();

        host.Run();
    }

    private static IHostBuilder CreateDefaultBuilder()
    {
        using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
            .SetMinimumLevel(LogLevel.Trace)
            .AddConsole());
        
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(app =>
            {
                app.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                var config = hostContext.Configuration;
                services.AddHostedService<Worker>()
                    .AddFacades()
                    .AddServices()
                    .AddAutoMapper()
                    .AddRabbitMQ()
                    .AddHubs()
                    .AddConfigurations(config)
                    .AddAuthentication(config)
                    .AddHttpClient(config);
            });
        
    }
}