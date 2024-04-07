using Examples.Service.Domain.Entities;

namespace Examples.Service.Persistence.Repositories
{
    public class Repository<TEntity>
        where TEntity : class, IEntity
    {
        protected static List<TEntity> entities = new();

        public virtual TEntity Add(TEntity entity)
        {
            entity.Id = entity.Id ?? Guid.NewGuid();
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            entities.Add(entity);
            return entity;
        }

        public virtual TEntity Get(Guid id)
        {
            return entities.Find(x => x.Id == id);
        }

        public virtual List<TEntity> GetAll()
        {
            return entities;
        }

        public virtual TEntity Update(TEntity entity)
        {
            var entityIndex = entities.FindIndex(x => x.Id == entity.Id);
            if (entityIndex >= 0)
            {
                entity.UpdateDate = DateTime.Now;
                entities.RemoveAt(entityIndex);
                entities.Insert(entityIndex, entity);
            }
            return entity;
        }

        public virtual bool Delete(Guid id)
        {
            var existingEntity = entities.Find(x => x.Id == id);
            if (existingEntity is null)
            {
                return false;
            }
            entities.Remove(existingEntity);
            return true;
        }
    }
}
