using JobsityChallenge.Chat.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace JobsityChallenge.Chat
{
    public class StockQueueConsumerService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var connection = new HubConnectionBuilder().WithUrl("https://localhost:7224/chat").Build();
                    await connection.StartAsync();
                    await connection.InvokeAsync("SendMessage", message);
                };

                while (true)
                {
                    channel.BasicConsume(queue: "stock-queue", autoAck: true, consumer: consumer);
                }
            }
            
            //return Task.CompletedTask;
        }
    }
}
