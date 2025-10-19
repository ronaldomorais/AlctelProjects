using Alctel.CRM.Context.InMemory.Context;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Alctel.CRM.Context.InMemory.Repositories;

public class ModuleRepository : BaseRepository<Module>, IModuleRepository
{
    public ModuleRepository(CRMContext context) : base(context)
    {
    }
}