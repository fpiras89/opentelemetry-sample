using Examples.Service.Application.Dtos;
using Examples.Service.Application.Interfaces;
using Examples.Service.Presentation.GraphQL.Todo;
using GraphQL;

namespace Examples.Service.Presentation.GraphQL
{
    public sealed partial class Mutation
    {
        [Scoped]
        public static async Task<TodoDto> AddTodo(
            [FromServices] ITodoService todoService,
            [FromServices] ILogger<Mutation> logger,
            TodoInput todo)
        {
            logger.LogInformation("Adding todo: {@todo}", todo);
            var result = await todoService.AddAsync(todo.ToDto());
            return result;
        }

        public static bool Nope() 
        {
            return true;
        }
    }
}
