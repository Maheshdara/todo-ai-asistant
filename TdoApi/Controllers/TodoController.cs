using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TdoApi.Interface;

namespace TdoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodo _todoService;
        public TodoController(ITodo todoService)
        {
            _todoService = todoService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTodos()
        {
            var todos = await _todoService.GetAllTodos();
            return Ok(todos);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoById(int id)
        {
            var todo = await _todoService.GetTodoById(id);
            if(todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] DTO.CreateTodos todo)
        {
            var newTodo = await _todoService.CreateTodo(todo);
            return Ok(newTodo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] DTO.UpdateTodos todo)
        {
            var updatedTodo = await _todoService.UpdateTodo(id, todo);
            if(updatedTodo == null)
            {
                return NotFound();
            }
            return Ok(updatedTodo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var deletedTodo = await _todoService.DeleteTodo(id);
            if(deletedTodo == null)
            {
                return NotFound();
            }
            return Ok(deletedTodo);
        }
    }
}
