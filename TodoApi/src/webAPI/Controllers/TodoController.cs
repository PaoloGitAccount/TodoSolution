//namespace WebAPI.Controllers;
namespace Infrastructure.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
//using Application.Services;
//using System.ComponentModel.DataAnnotations;
//using Autofac;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Versioning;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TodoController : ControllerBase
{
    private readonly TodoService _todoService;

    public TodoController(TodoService todoService) => _todoService = todoService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
    {
        var todos = await _todoService.GetTodosAsync();
        return Ok(todos);
    }

    [HttpPost]
    public async Task<ActionResult> CreateTodoItem(TodoItem todoItem)
    {
        await _todoService.AddTodoAsync(todoItem);
        return Ok();
    }

    [HttpGet("by-priority/{priority}")]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodosByPriority(int priority)
    {
        var todos = await _todoService.GetTodosByPriorityAsync(priority);
        return Ok(todos);
    }
}
