using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
namespace Alctel.CRM.Context.InMemory.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<List<T>?> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<bool> InsertAsync(T data);
    Task<bool> UpdateAsync(T data);
    Task<bool> DeleteAsync(T data);
    Task<List<T>?> FindAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<bool> DeleteInsertAsync(Int64 Id, T data);
}