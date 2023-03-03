using System.Diagnostics.CodeAnalysis;
using Chat.Bot.Service.Services;
using Chat.Bot.Service.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Bot.Service.Di
{
    [ExcludeFromCodeCoverage]
    public static class ServicesDi
    {
        public static IServiceCollection AddServices(this IServiceCollection services) =>
            services.AddTransient<IStockService, StockService>();
    }
}