using Examples.Service.Domain.Entities;

namespace Examples.Service.Domain.Interfaces
{
    public interface IDbContext
    {
        IQueryable<TEntity> Query<TEntity>() where TEntity : class;
        TEntity Add<TEntity>(TEntity entity) where TEntity : class;
        TEntity Update<TEntity>(TEntity entity) where TEntity : class;
        bool Remove<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
