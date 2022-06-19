using JobsityChallenge.Chat.Settings;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace JobsityChallenge.Chat;

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
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var factory = new ConnectionFactory() { HostName = _rabbitHost };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var connection = new HubConnectionBuilder().WithUrl($"{_applicationHostName}/chat").Build();
                await connection.StartAsync();
                await connection.InvokeAsync("SendMessage", message);
            };
            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            //while (true)
            //{
            //    channel.BasicConsume(queue: "stock-queue", autoAck: true, consumer: consumer);
            //}
        }

        return Task.CompletedTask;
    }
}
