using Alctel.CRM.Context.InMemory.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface IActionPermissionService
{
    Task<List<ActionPermission>?> GetAllActionPermissionAsync();
    Task<ActionPermission?> GetActionPermissionAsync(Int64 Id);
    Task<bool> CreateActionPermissionAsync(ActionPermission actionPermission);
    Task<bool> UpdateActionPermissionAsync(ActionPermission actionPermission);
    Task<bool> DeleteActionPermissionAsync(ActionPermission actionPermission);
}