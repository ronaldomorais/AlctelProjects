using Alctel.CRM.Context.InMemory.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface IModuleService
{
    Task<List<Module>?> GetAllModuleAsync();
    Task<Module?> GetModuleAsync(Int64 Id);
    Task<bool> CreateModuleAsync(Module module);
    Task<bool> UpdateModuleAsync(Module module);
    Task<bool> DeleteModuleAsync(Module module);
}