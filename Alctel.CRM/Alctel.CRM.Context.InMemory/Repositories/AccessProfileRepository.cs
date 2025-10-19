using System.ComponentModel;
using Alctel.CRM.Context.InMemory.Context;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Alctel.CRM.Context.InMemory.Repositories;

public class AccessProfileRepository : BaseRepository<AccessProfile>, IAccessProfileRepository
{
    private readonly CRMContext _context;
    private readonly CRMContext _dbcontext;

    public AccessProfileRepository(CRMContext context, CRMContext dbcontext) : base(context)
    {
        _context = context;
        _dbcontext = dbcontext;
    }

    public async Task<bool> UpdateAccessProfileAsync(AccessProfile accessProfile)
    {
        try
        {
            var accessProfileFound = _context.AccessProfiles.FirstOrDefault(_ => _.Id == accessProfile.Id);

            if (accessProfileFound != null)
            {
                _context.Remove(accessProfileFound);
                await _context.SaveChangesAsync();
                _dbcontext.Add(accessProfile);
                await _dbcontext.SaveChangesAsync();

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }
}