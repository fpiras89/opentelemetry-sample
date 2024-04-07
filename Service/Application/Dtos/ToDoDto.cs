using Examples.Service.Domain.Entities;

namespace Examples.Service.Application.Dtos
{
    public class ToDoDto
    {
        public static ToDo ToEntity()
        {
            return new ToDo { };
        }

        public static ToDoDto FromEntity(ToDo todoEntity)
        {
            return new ToDoDto { };
        }
    }
}
