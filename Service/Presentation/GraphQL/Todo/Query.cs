using Examples.Service.Application.Dtos;
using Examples.Service.Application.Interfaces;
using GraphQL;

namespace Examples.Service.Presentation.GraphQL
{
    public sealed partial class Query
    {
        [Scoped]
        public static async Task<TodoDto> Todo(
            [FromServices] ITodoService todoService,
            Guid id)
        {
            return await todoService.GetAsync(id);
        }

        [Scoped]
        public static async Task<List<TodoDto>> Todos(
            [FromServices] ITodoService todoService)
        {
            return await todoService.GetAllAsync();
        }
    }
}
