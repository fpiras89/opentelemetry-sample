using Examples.Service.Domain.Entities;
using Examples.Service.Domain.Interfaces;
using Examples.Service.Persistence.Repositories;
using GraphQL.Conventions;
using Microsoft.EntityFrameworkCore;

namespace Examples.Service.Presentation.GraphQL
{
    public sealed partial class Query
    {
        public async Task<ToDo> ToDo([Inject] IDbContext dbContext, Guid id)
        {
            return await dbContext.Query<ToDo>().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<ToDo>> ToDos([Inject] IDbContext dbContext)
        {
            return await dbContext.Query<ToDo>().ToListAsync();
        }
    }
}
