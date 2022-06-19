using Microsoft.EntityFrameworkCore.Query;

namespace JobsityChallenge.Chat.Repositories
{
    public interface IRepository<TEntity, TID>
    {
        public Task<TEntity> GetByIdAsync(TID id);
        public Task AddAsync(TEntity obj);
        public Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                      int? take = null);
        public Task Save();
    }
}
