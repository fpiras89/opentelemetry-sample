using Examples.Service.Domain.Entities;

namespace Examples.Service.Persistence.Repositories
{
    public class TodoRepository : Repository<TodoEntity>
    {
        public TodoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
