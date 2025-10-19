using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.Context.InMemory.Context;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Alctel.CRM.Context.InMemory.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly CRMContext _context;

    public UserRepository(CRMContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        try
        {
            return await _context.Users
                .Include(_ => _.AccessProfiles)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<User>();
    }
}
