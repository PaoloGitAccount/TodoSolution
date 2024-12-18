namespace Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TodoService
{
    private readonly IUnitOfWork _unitOfWork;

    public TodoService(IUnitOfWork unitOfWork) => _unitOfWork
    = unitOfWork;

    public Task<IEnumerable<TodoItem>> GetTodosAsync() 
        => _unitOfWork.TodoRepository.ListAsync();

    public async Task AddTodoAsync(TodoItem todoItem)
    {
        await _unitOfWork.TodoRepository.AddAsync(todoItem);
        await _unitOfWork.CompleteAsync();
    }

    public Task<IEnumerable<TodoItem>> GetTodosByPriorityAsync(int priority) 
        => _unitOfWork.TodoRepository.ListByPriorityAsync(priority);
}
