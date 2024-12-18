namespace Core.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

public interface ITodoRepository : IRepository<TodoItem>
{
    Task<IEnumerable<TodoItem>>
ListByPriorityAsync(int priority);
}
