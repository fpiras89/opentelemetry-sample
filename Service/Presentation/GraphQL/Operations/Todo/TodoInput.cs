using Examples.Service.Application.Dtos;
using GraphQL.Conventions;

namespace Examples.Service.Presentation.GraphQL
{
    [InputType]
    public class TodoInput : TodoDto
    {
        [Ignore]
        public TodoDto ToDto()
        {
            return this;
        }
    }
}
