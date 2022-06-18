namespace JobsityChallenge.Chat.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public UserModel User { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public MessageModel(string userId, string text)
        {
            UserId = userId;
            Text = text;
        }
    }
}
