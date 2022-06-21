namespace JobsityChallenge.Chat.Entities
{
    public class ChatroomEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MessageEntity>? Messages { get; set; }
    }
}
