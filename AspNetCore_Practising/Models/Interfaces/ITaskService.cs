namespace AspNetCore_Practising.Models.Interfaces
{
    public interface ITaskService
    {
        Todo_Model? GetTodoById(int id);
        List<Todo_Model> GetTodos();
        void DeleteTodoById(int id);
        Todo_Model AddTodo(Todo_Model todo);
    }
}
