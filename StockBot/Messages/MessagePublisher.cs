using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace StockBot.Messages
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IConnectionFactory _connectionFactory;
        public MessagePublisher(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void PublishMessageOnQueue(string queueName, object message)
        {
            using(var connection = _connectionFactory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queueName);
                    var stringfiedMessage = JsonConvert.SerializeObject(message);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);
                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: bytesMessage);
                }
            }
        }
    }
}
