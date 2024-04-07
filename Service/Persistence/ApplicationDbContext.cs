using Examples.Service.Domain.Entities;
using Examples.Service.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Examples.Service.Persistence
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        #region DbSets

        public DbSet<ToDo> ToDos { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public IQueryable<TEntity> Query<TEntity>()
             where TEntity : class
        {
            return Set<TEntity>().AsQueryable();
        }

        public new TEntity Add<TEntity>(TEntity entity)
             where TEntity : class
        {
            base.Add(entity);
            return entity;
        }

        public new TEntity Update<TEntity>(TEntity entity)
             where TEntity : class
        {
            base.Update(entity);
            return entity;
        }

        public new bool Remove<TEntity>(TEntity entity)
             where TEntity : class
        {
            base.Remove(entity);
            return true;
        }
    }
}
