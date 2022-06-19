using JobsityChallenge.Chat.Entities;

namespace JobsityChallenge.Chat.Repositories
{
    public interface IUserRepository
    {
        public Task<UserEntity> GetByIdAsync(string id);
    }
}
