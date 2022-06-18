namespace JobsityChallenge.Chat.Entities
{
    public class MessageEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public UserEntity User { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public MessageEntity(string userId, string text)
        {
            UserId = userId;
            Text = text;
        }
    }
}
