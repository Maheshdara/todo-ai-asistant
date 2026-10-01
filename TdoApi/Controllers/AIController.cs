using Microsoft.AspNetCore.Mvc;
using TdoApi.DTO;
using TdoApi.Interface;

namespace TdoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly IGemini _geminiService;
        private readonly ITodo _todoService;

        public AIController(IGemini geminiService, ITodo todoService)
        {
            _geminiService = geminiService;
            _todoService = todoService;
        }

        [HttpPost("chat")]

        public async Task<IActionResult> Chat(
            AIChatDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Message))
            {
                return BadRequest(new
                {
                    message = "Please enter a message."
                });
            }

            // Get existing Todos
            var todos =
                await _todoService.GetAllTodos();

            // Ask Gemini to understand request
            var command =
                await _geminiService.UnderstandCommand(
                    dto.Message,
                    todos);

            // CREATE
            if (command.Action == "create")
            {
                var createDto = new CreateTodos
                {
                    Title = command.Title ?? "",
                    Description = command.Description,
                    DueDate = command.DueDate
                };

                var todo =
                    await _todoService.CreateTodo(
                        createDto);

                return Ok(new
                {
                    message =
                        $"✅ Todo '{todo.Title}' created successfully.",
                    data = todo
                });
            }
             
            // GET
            if (command.Action == "get")
            {
                var result =
                    await _todoService.SearchTodos(
                        command.SearchText,
                        command.IsCompleted,
                        command.DateFilter);

                return Ok(new
                {
                    message =
                        $"📋 Found {result.Count()} todo(s).",

                    data = result
                });
            }

            // DELETE
            if (command.Action == "delete")
            {
                if (command.TodoId == null)
                {
                    return Ok(new
                    {
                        message =
                            "I couldn't identify which Todo you want to delete."
                    });
                }

                var todo =
                    await _todoService.GetTodoById(
                        command.TodoId.Value);

                if (todo == null)
                {
                    return NotFound(new
                    {
                        message = "Todo not found."
                    });
                }

                await _todoService.DeleteTodo(
                    command.TodoId.Value);

                return Ok(new
                {
                    message =
                        $"🗑️ Todo '{todo.Title}' deleted successfully."
                });
            }

            // UPDATE
            if (command.Action == "update")
            {
                if (command.TodoId == null)
                {
                    return Ok(new
                    {
                        message =
                            "I couldn't identify which Todo you want to update."
                    });
                }

                var existingTodo =
                    await _todoService.GetTodoById(
                        command.TodoId.Value);

                if (existingTodo == null)
                {
                    return NotFound(new
                    {
                        message = "Todo not found."
                    });
                }

                var updateDto = new UpdateTodos
                {
                    Title =
                        command.NewTitle
                        ?? existingTodo.Title,

                    Description =
                        command.NewDescription
                        ?? existingTodo.Description,

                    IsCompleted =
                        command.IsCompleted
                        ?? existingTodo.IsCompleted,

                    DueDate =
                        command.DueDate
                        ?? existingTodo.DueDate
                };

                var updatedTodo =
                    await _todoService.UpdateTodo(
                        command.TodoId.Value,
                        updateDto);

                return Ok(new
                {
                    message =
                        $"✅ Todo '{updatedTodo!.Title}' updated successfully.",

                    data = updatedTodo
                });
            }

            return Ok(new
            {
                message =
                    "I couldn't understand your Todo request."
            });
        }
    }
}