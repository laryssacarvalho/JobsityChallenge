namespace JobsityChallenge.Chat.Entities;

public class MessageEntity
{
    public int Id { get; private set; }
    public string UserId { get; private set; }
    public UserEntity User { get; private set; }
    public string Text { get; private set; }
    public DateTime Date { get; private set; }
    public MessageEntity() { }
    public MessageEntity(string userId, string text)
    {
        UserId = userId;
        Text = text;
    }
}
