using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class TodoRepository : EfRepository<TodoItem>, ITodoRepository
    {
        public TodoRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<TodoItem>> ListByPriorityAsync(int priority)
        {
            return await _dbContext.TodoItems.Where(item => item.Priority == priority).ToListAsync();
        }
    }
}


