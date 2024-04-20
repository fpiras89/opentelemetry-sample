using Examples.Service.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Examples.Service.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<TEntity> entities;

        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entities = dbContext.Set<TEntity>();
        }

        public virtual Task<TEntity> AddAsync(TEntity entity)
        {
            entity.Id = entity.Id ?? Guid.NewGuid();
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            dbContext.Add(entity);
            return Task.FromResult(entity);
        }

        public virtual Task<TEntity> GetAsync(Guid id)
        {
            return entities.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual Task<List<TEntity>> GetAllAsync()
        {
            return entities.ToListAsync();
        }

        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            dbContext.Attach(entity);
            dbContext.Update(entity);
            return Task.FromResult(entity);
        }

        public async virtual Task<bool> DeleteAsync(Guid id)
        {
            var existingEntity = await entities.FirstOrDefaultAsync(x => x.Id == id);
            if (existingEntity == null)
            {
                return false;
            }
            entities.Remove(existingEntity);
            return true;
        }
    }
}
