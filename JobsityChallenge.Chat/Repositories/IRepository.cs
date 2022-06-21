using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace JobsityChallenge.Chat.Repositories;

public interface IRepository<TEntity, TID>
{
    public Task<TEntity> GetByIdAsync(TID id);
    public Task AddAsync(TEntity obj);
    public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                  int? take = null);
    public Task SaveAsync();
}
