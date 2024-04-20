using Examples.Service.Application.Dtos;
using Examples.Service.Application.Interfaces;
using GraphQL.Conventions;

namespace Examples.Service.Presentation.GraphQL
{
    public sealed partial class Mutation
    {
        public async Task<TodoDto> AddTodo([Inject] ITodoService todoService, TodoInput todo)
        {
            return await todoService.AddAsync(todo.ToDto());
        }

        public bool Nope() 
        {
            return true;
        }
    }
}
