using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace JobsityChallenge.Bot.Messages;

public class MessagePublisher : IMessagePublisher
{
    private readonly IConnectionFactory _connectionFactory;
    public MessagePublisher(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void PublishMessageOnQueue(string queueName, string host, object message)
    {

        var factory = new ConnectionFactory() { HostName = host };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())        
        {
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);
            var stringfiedMessage = JsonConvert.SerializeObject(message);
            var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: bytesMessage);
        }        
    }
}
