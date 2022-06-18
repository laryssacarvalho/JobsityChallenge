using JobsityChallenge.Chat.Entities;

namespace JobsityChallenge.Chat.Repositories
{
    public interface IMessageRepository
    {
        public Task<IEnumerable<MessageEntity>> GetMessages();
        public Task InsertMessage(MessageEntity message);
        public Task Save();
    }
}
