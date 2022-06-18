using JobsityChallenge.Chat.Models;
using Microsoft.EntityFrameworkCore;

namespace JobsityChallenge.Chat.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private DbContext _context;
        private DbSet<MessageModel> _dbSet;

        public MessageRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<MessageModel>();
        }

        public async Task<IEnumerable<MessageModel>> GetMessages() => await _dbSet.ToListAsync();

        public async Task InsertMessage(MessageModel message) => await _dbSet.AddAsync(message);                

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
}
