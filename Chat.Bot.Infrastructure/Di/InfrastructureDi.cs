using System.Diagnostics.CodeAnalysis;
using System.Text;
using Chat.Bot.Domain.Config;
using Chat.Bot.Infrastructure.Clients;
using Chat.Bot.Infrastructure.Facade;
using Chat.Bot.Infrastructure.Facade.Interfaces;
using Chat.Bot.Infrastructure.Interfaces.Facade;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Bot.Infrastructure.Di
{
    [ExcludeFromCodeCoverage]
    public static class InfrastructureDi
    {
        public static IServiceCollection AddFacades(this IServiceCollection services) =>
            services.AddTransient<IStockFacade, StockFacade>();
        public static IServiceCollection AddAutoMapper(this IServiceCollection services) =>
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        public static void AddHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration.GetSection("StockConfig:StockURL").Value;

            if (string.IsNullOrEmpty(url))
                throw new Exception("URL Stock is empty on AppSettings.");
            
            services.AddHttpClient("StockClient", options =>
            {
                options.BaseAddress = new Uri(url);
            });
        }
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration.GetSection("Key:JWT").Value;
            if (string.IsNullOrEmpty(key))
                throw new Exception("JWT:Key is empty on AppSettings.");

            services
                .AddAuthentication(config =>
                {
                    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            return services;
        }
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
            => services.AddTransient<IRabbitMQFacade, RabbitMQFacade>();
        public static IServiceCollection AddHubs(this IServiceCollection services) =>
            services.AddTransient<IHubChatRoom, HubChatRoom>();
        public static IServiceCollection AddConfigurations(this IServiceCollection services,
            IConfiguration configuration) =>
            services.Configure<RabbitMQConfig>(configuration.GetSection("RabbitMQ"))
                .Configure<HubConnectionConfig>(configuration.GetSection("Hub"));
    }
}