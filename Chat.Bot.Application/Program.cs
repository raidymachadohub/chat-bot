using Chat.Bot.Infrastructure.Di;
using Chat.Bot.Service.Di;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Host.ConfigureServices((hostContext, services) =>
{
    var config = hostContext.Configuration;
    services
        .AddFacades()
        .AddServices()
        .AddAutoMapper()
        .AddRabbitMQ()
        .AddAuthentication(config)
        .AddHttpClient(config);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();