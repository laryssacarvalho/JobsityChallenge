using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace JobsityChallenge.Chat.Repositories;

public class Repository<TEntity, TID> : IRepository<TEntity, TID> where TEntity : class, new()
{
    protected readonly DbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(DbContext context)
    {
        DbContext = context;
        DbSet = DbContext.Set<TEntity>();
    }

    public async Task<TEntity> GetByIdAsync(TID id) => await DbSet.FindAsync(id);
    public async Task AddAsync(TEntity obj) => await DbSet.AddAsync(obj);
    public async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                      int? take = null)
    {
        var query = DbSet.AsQueryable();

        if (include is not null)
            query = include(query);

        if (orderBy is not null)
            query = orderBy(query);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync();
    }

    public async Task Save()
    {
        await DbContext.SaveChangesAsync();
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
                DbContext.Dispose();
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
