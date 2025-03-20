using AspNetCore_Practising.Models;
using AspNetCore_Practising.Models.Interfaces;

namespace AspNetCore_Practising.Services
{
    public class InMemoryTaskService : ITaskService
    {
        private readonly List<Todo_Model> _todos = [];
        public Todo_Model AddTodo(Todo_Model todo)
        {
            _todos.Add(todo);
            return todo;
        }

        public void DeleteTodoById(int id) => _todos.RemoveAll(todo => todo.Id == id);

        public Todo_Model? GetTodoById(int id) => _todos.FirstOrDefault(el  => el.Id == id);

        public List<Todo_Model> GetTodos() => _todos;
    }
}
