using Examples.Service.Application.Dtos;
using GraphQL;

namespace Examples.Service.Presentation.GraphQL.Todo
{
    public class TodoInput : TodoDto
    {
        [Ignore]
        public TodoDto ToDto()
        {
            return this;
        }
    }
}
