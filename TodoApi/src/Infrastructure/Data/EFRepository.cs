namespace Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EfRepository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _dbContext;

    public EfRepository(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task<T?> GetByIdAsync(int id) 
        => await _dbContext.Set<T>().FindAsync(id);

    public async Task<IEnumerable<T>> ListAsync() 
        => await _dbContext.Set<T>().ToListAsync();

    public async Task AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}
