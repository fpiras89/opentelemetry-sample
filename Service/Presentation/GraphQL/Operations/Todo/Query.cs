using Examples.Service.Application.Dtos;
using Examples.Service.Application.Interfaces;
using Examples.Service.Presentation.GraphQL.Attributes;
using GraphQL.Conventions;

namespace Examples.Service.Presentation.GraphQL
{
    public sealed partial class Query
    {
        [Scoped]
        public async Task<TodoDto> Todo([Inject] ITodoService todoService, Guid id)
        {
            return await todoService.GetAsync(id);
        }

        [Scoped]
        public async Task<List<TodoDto>> Todos([Inject] ITodoService todoService)
        {
            return await todoService.GetAllAsync();
        }
    }
}
