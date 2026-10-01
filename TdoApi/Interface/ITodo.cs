using TdoApi.DTO;
using TdoApi.Models;

namespace TdoApi.Interface
{
    public interface ITodo
    {
        public Task<IEnumerable<TodoTask>> GetAllTodos();

        public Task<TodoTask> GetTodoById(int id);

        public Task<TodoTask> CreateTodo(CreateTodos todo);

        public Task<TodoTask> UpdateTodo(int id, UpdateTodos todo);

        public Task<TodoTask> DeleteTodo(int id);

        Task<IEnumerable<TodoTask>> SearchTodos(string? searchText, bool? isCompleted, string? dateFilter);
    }
}
