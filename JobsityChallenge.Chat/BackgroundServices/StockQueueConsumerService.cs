using JobsityChallenge.Chat.Events;
using JobsityChallenge.Chat.Hubs;
using JobsityChallenge.Chat.Settings;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace JobsityChallenge.Chat.BackgroundServices;

public class StockQueueConsumerService : BackgroundService
{
    private readonly string _queueName;
    private readonly string _rabbitHost;
    private readonly string _applicationHostName = "https://localhost:7224";

    public StockQueueConsumerService(IOptions<ApplicationSettings> settings)
    {
        _queueName = settings.Value.StockQueueName;
        _rabbitHost = settings.Value.RabbitMqHost;        
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory() { HostName = _rabbitHost };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageString = Encoding.UTF8.GetString(body);

                var message = JsonConvert.DeserializeObject<StockQuoteResponseEvent>(messageString);
                
                var connection = new HubConnectionBuilder().WithUrl($"{_applicationHostName}/chat").Build();
                
                await connection.StartAsync();
                await connection.InvokeAsync("JoinGroup", message.ChatId);
                await connection.InvokeAsync("SendMessage", message.Text, message.ChatId, null);
                
            };

            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
