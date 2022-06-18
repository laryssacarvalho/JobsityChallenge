using JobsityChallenge.Chat.Models;

namespace JobsityChallenge.Chat.Repositories
{
    public interface IMessageRepository
    {
        public Task<IEnumerable<MessageModel>> GetMessages();
        public Task InsertMessage(MessageModel message);
        public Task Save();
    }
}
