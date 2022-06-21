namespace JobsityChallenge.Bot.Messages
{
    public class StockQuoteResponseMessage
    {
        public string Text { get; private set; }
        public int ChatId { get; private set; }
        public StockQuoteResponseMessage(string text, int chatId)
        {
            Text = text;
            ChatId = chatId;
        }
    }
}
