using Examples.Service.Entities;

namespace Examples.Service.Repositories
{
    public class ToDoRepository
    {
        private static List<ToDo> todos = new ();

        public static ToDo Add(ToDo todo)
        {
            todos.Add(todo);
            return todo;
        }

        public static ToDo Get(Guid id)
        {
            return todos.Find(x => x.Id == id);
        }

        public static List<ToDo> GetAll()
        {
            return todos;
        }

        public static ToDo Update(ToDo todo)
        {
            var existingTodo = todos.Find(x => x.Id == todo.Id);
            if (existingTodo is not null)
            {
                existingTodo.Text = todo.Text;
                existingTodo.Done = todo.Done;
            }
            return existingTodo;
        }

        public static bool Delete(Guid id)
        {
            var existingTodo = todos.Find(x => x.Id == id);
            if (existingTodo is null)
            {
                return false;
            }
            todos.Remove(existingTodo);
            return true;
        }
    }

}
