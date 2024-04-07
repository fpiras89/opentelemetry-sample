using Examples.Service.Domain.Entities;
using GraphQL.Conventions;

namespace Examples.Service.Presentation.GraphQL.Inputs
{
    [InputType]
    public class ToDoInput : ToDo
    {
        [Ignore]
        public ToDo ToEntity()
        {
            return this;
        }
    }
}
