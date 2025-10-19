using Alctel.CRM.Context.InMemory.Entities;

namespace Alctel.CRM.Context.InMemory.Interfaces;

public interface IAccessProfileRepository : IBaseRepository<AccessProfile>
{
    Task<bool> UpdateAccessProfileAsync(AccessProfile accessProfile);
}