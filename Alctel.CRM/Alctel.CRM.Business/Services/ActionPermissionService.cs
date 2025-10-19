using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Interfaces;

namespace Alctel.CRM.Business.Services;

public class ActionPermissionService : IActionPermissionService
{
    private readonly IActionPermissionRepository _actionPermissionRepository;
    public ActionPermissionService(IActionPermissionRepository actionPermissionRepository)
    {
        _actionPermissionRepository = actionPermissionRepository;
    }

    public async Task<List<ActionPermission>?> GetAllActionPermissionAsync()
    {
        try
        {
            return await _actionPermissionRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<ActionPermission?> GetActionPermissionAsync(Int64 id)
    {
        try
        {
            var actionPermissions = await _actionPermissionRepository.FindAsync(_ => _.Id == id);

            if (actionPermissions != null && actionPermissions.Any())
            {
                return actionPermissions.FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<bool> CreateActionPermissionAsync(ActionPermission actionPermission)
    {
        try
        {
            if (actionPermission != null)
            {
                actionPermission.ActionChecked = true;
                return await _actionPermissionRepository.InsertAsync(actionPermission);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> UpdateActionPermissionAsync(ActionPermission actionPermission)
    {
        try
        {
            return await _actionPermissionRepository.UpdateAsync(actionPermission);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }       
    
    public async Task<bool> DeleteActionPermissionAsync(ActionPermission actionPermission)
    {
        try
        {
            return await _actionPermissionRepository.DeleteAsync(actionPermission);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }      
}