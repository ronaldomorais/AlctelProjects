using Alctel.CRM.API.Entities;
using Alctel.CRM.Context.InMemory.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface IAccessProfileService
{
    Task<List<AccessProfile>?> GetAllAccessProfileAsync();
    Task<AccessProfile?> GetAccessProfileAsync(Int64 Id);
    Task<bool> CreateAccessProfileAsync(AccessProfile accessProfile);
    Task<bool> UpdateAccessProfileAsync(AccessProfile accessProfile);
    Task<bool> DeleteAccessProfileAsync(AccessProfile accessProfile);

    Task<List<AccessProfileAPI>> GetAllAccessProfileAPIAsync();
    Task<List<AccessProfileAPI>> GetAccessProfileActivatedListAPIAsync();
}