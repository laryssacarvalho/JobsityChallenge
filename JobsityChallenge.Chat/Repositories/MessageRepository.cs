using JobsityChallenge.Chat.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobsityChallenge.Chat.Repositories;

public class MessageRepository : IMessageRepository
{
    private DbContext _context;
    private DbSet<MessageEntity> _dbSet;

    public MessageRepository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<MessageEntity>();
    }

    public async Task<IEnumerable<MessageEntity>> GetMessages() => await _dbSet.Include(m => m.User).ToListAsync();

    public async Task InsertMessage(MessageEntity message) => await _dbSet.AddAsync(message);                

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

