namespace JobsityChallenge.Bot.Messages;

public interface IMessagePublisher
{
    public void PublishMessageOnQueue(string queueName, string host, object message);
}
