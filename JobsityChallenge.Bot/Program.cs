using RabbitMQ.Client;
using JobsityChallenge.Bot.Messages;
using JobsityChallenge.Bot.Services;
using JobsityChallenge.Bot.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICommandService, CommandService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IConnectionFactory, ConnectionFactory>();
builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();

builder.Services.AddHttpClient<IStockService, StockService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
