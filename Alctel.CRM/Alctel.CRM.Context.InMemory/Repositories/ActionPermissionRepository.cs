using Alctel.CRM.Context.InMemory.Context;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Interfaces;

namespace Alctel.CRM.Context.InMemory.Repositories;

public class ActionPermissionRepository : BaseRepository<ActionPermission>, IActionPermissionRepository
{
    public ActionPermissionRepository(CRMContext context) : base(context)
    {
    }
}