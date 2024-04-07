using Examples.Service.Domain.Entities;
using Examples.Service.Domain.Interfaces;
using Examples.Service.Presentation.GraphQL.Inputs;
using GraphQL.Conventions;

namespace Examples.Service.Presentation.GraphQL
{
    public sealed partial class Mutation
    {
        public async Task<ToDo> AddToDo([Inject] IDbContext dbContext, ToDoInput todo)
        {
            var result = dbContext.Add(todo.ToEntity());
            await dbContext.SaveChangesAsync();
            return result;
        }

        public bool Nope() 
        {
            return true;
        }
    }
}
