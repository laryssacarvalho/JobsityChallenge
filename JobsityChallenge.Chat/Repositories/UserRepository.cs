using JobsityChallenge.Chat.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobsityChallenge.Chat.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DbContext _context;
        private DbSet<UserEntity> _dbSet;

        public UserRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<UserEntity>();
        }

        public async Task<UserEntity> GetByIdAsync(string id) => await _dbSet.FindAsync(id);

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
    }
}
