namespace JobsityChallenge.Bot.Messages;

public interface IMessagePublisher
{
    public void PublishMessageOnQueue(string queueName, object message);
}
