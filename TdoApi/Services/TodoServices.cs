using Microsoft.EntityFrameworkCore;
using TdoApi.DTO;
using TdoApi.Interface;
using TdoApi.Models;

namespace TdoApi.Services
{
    public class TodoServices : ITodo
    {

        private readonly ApplicationDbContext _context;

        public TodoServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public  async Task<IEnumerable<TodoTask>> GetAllTodos()
        {
            var todos = await _context.TodoTasks.ToListAsync();
            return todos;
        }

        public async Task<TodoTask> GetTodoById(int id)
        {
            var todo = await _context.TodoTasks.FindAsync(id);
            return todo!;
        }

        public async Task<TodoTask> CreateTodo(CreateTodos todo)
        {
            var newTodo = new TodoTask
            {
                Title = todo.Title,
                Description = todo.Description,
                DueDate = todo.DueDate,
                CreatedDate = DateTime.Now
            };
            _context.TodoTasks.Add(newTodo);
            await _context.SaveChangesAsync();
            return newTodo;
        }

        public async Task<TodoTask> UpdateTodo(int id, UpdateTodos todo)
        {
            var existingTodo = await _context.TodoTasks.FindAsync(id);
            if (existingTodo == null)
            {
                return null!;
            }
            existingTodo.Title = todo.Title;
            existingTodo.Description = todo.Description;
            existingTodo.IsCompleted = todo.IsCompleted;
            existingTodo.DueDate = todo.DueDate;
            await _context.SaveChangesAsync();
            return existingTodo;
        }

        public async Task<TodoTask> DeleteTodo(int id)
        {
            var existingTodo = await _context.TodoTasks.FindAsync(id);
            if (existingTodo == null)
            {
                return null!;
            }
            _context.TodoTasks.Remove(existingTodo);
            await _context.SaveChangesAsync();
            return existingTodo;
        }



        public async Task<IEnumerable<TodoTask>> SearchTodos(
    string? searchText,
    bool? isCompleted,
    string? dateFilter)
        {
            var query = _context.TodoTasks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(x =>
                    x.Title.Contains(searchText) ||
                    (x.Description != null &&
                     x.Description.Contains(searchText)));
            }

            if (isCompleted.HasValue)
            {
                query = query.Where(x =>
                    x.IsCompleted == isCompleted.Value);
            }

            if (dateFilter == "today")
            {
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);

                query = query.Where(x =>
                    x.DueDate >= today &&
                    x.DueDate < tomorrow);
            }

            if (dateFilter == "tomorrow")
            {
                var tomorrow = DateTime.Today.AddDays(1);
                var dayAfterTomorrow = tomorrow.AddDays(1);

                query = query.Where(x =>
                    x.DueDate >= tomorrow &&
                    x.DueDate < dayAfterTomorrow);
            }

            if (dateFilter == "overdue")
            {
                query = query.Where(x =>
                    x.DueDate < DateTime.Now &&
                    !x.IsCompleted);
            }

            return await query
                .OrderBy(x => x.DueDate)
                .ToListAsync();
        }
    }



}
