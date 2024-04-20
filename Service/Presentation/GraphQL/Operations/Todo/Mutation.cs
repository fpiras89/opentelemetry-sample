using Examples.Service.Application.Dtos;
using Examples.Service.Application.Interfaces;
using Examples.Service.Presentation.GraphQL.Attributes;
using GraphQL.Conventions;

namespace Examples.Service.Presentation.GraphQL
{
    public sealed partial class Mutation
    {
        [Scoped]
        public async Task<TodoDto> AddTodo(
            [Inject] ITodoService todoService, [Inject] ILogger<Mutation> logger,
            TodoInput todo)
        {
            logger.LogInformation("Adding todo: {todo}", todo);
            return await todoService.AddAsync(todo.ToDto());
        }

        public bool Nope() 
        {
            return true;
        }
    }
}
