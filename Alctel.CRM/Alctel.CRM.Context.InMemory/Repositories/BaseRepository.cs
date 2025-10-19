

using System.Linq.Expressions;
using Alctel.CRM.Context.InMemory.Context;
using Alctel.CRM.Context.InMemory.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Alctel.CRM.Context.InMemory.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
{
    private readonly CRMContext _context;

    public BaseRepository(CRMContext context)
    {
        _context = context;
    }

    public async Task<List<T>?> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        try
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<bool> InsertAsync(T data)
    {
        try
        {
            _context.Set<T>().Add(data);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        { 
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");            
        }

        return false;
    }

    public async Task<bool> UpdateAsync(T data)
    {
        try
        {
            _context.Entry(data).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        { 
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> DeleteAsync(T data)
    {
        try
        {
            _context.Set<T>().Remove(data);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        { 
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<List<T>?> FindAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        try
        {
            //return await _context.Set<T>().Where(predicate).ToListAsync();

            try
            {
                IQueryable<T> query = _context.Set<T>().Where(predicate);

                if (include != null)
                {
                    query = include(query);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
            }

            return null;
        }
        catch (Exception ex)
        { 
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;        
    }

    public async Task<bool> DeleteInsertAsync(Int64 Id, T data)
    {
        try
        {
            //data.Id = id;
            _context.Set<T>().Update(data);
            await _context.SaveChangesAsync();
            _context.Entry(data).State = EntityState.Detached; //detach saved entity
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }
        return false;
    }

    //public async Task<int> Update<T>(T entity) where T : BaseEntity
    //{
    //    entity.UpdatedAt = DateTime.UtcNow;

    //    // Untrack previous entity version
    //    var trackedEntity = _context.Set<T>()
    //        .SingleOrDefaultAsync(e => e.Id == entity.Id);
    //    _context.Entry<T>(await trackedEntity).State = EntityState.Detached;

    //    // Track new version
    //    _context.Set<T>().Attach(entity);
    //    _context.Entry<T>(entity).State = EntityState.Modified;

    //    await _context.SaveChangesAsync();

    //    return entity.Id;
    //}
}