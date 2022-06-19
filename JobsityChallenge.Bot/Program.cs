using JobsityChallenge.Bot.Messages;
using JobsityChallenge.Bot.Services;
using JobsityChallenge.Bot.Services.Interfaces;
using JobsityChallenge.Bot.Settings;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICommandService, CommandService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IConnectionFactory, ConnectionFactory>();
builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();

builder.Services.AddHttpClient<IStockService, StockService>();

builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
