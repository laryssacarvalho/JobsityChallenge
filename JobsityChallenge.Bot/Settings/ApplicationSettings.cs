namespace JobsityChallenge.Bot.Settings
{
    public class ApplicationSettings
    {
        public string StockApiEndpoint { get; set; }
        public string RabbitMqHost { get; set; }
        public string StockQueueName { get; set; }
    }
}
