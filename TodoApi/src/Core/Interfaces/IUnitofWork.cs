namespace Core.Interfaces;
using System.Threading.Tasks;
using System;
    
public interface IUnitOfWork : IDisposable
{
    ITodoRepository TodoRepository { get; }
    Task<int> CompleteAsync();
}
