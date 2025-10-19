using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Alctel.CRM.Business.Services;

public class ModuleService : IModuleService
{
    private readonly IModuleRepository _moduleRepository;
    public ModuleService(IModuleRepository moduleRepository)
    {
        _moduleRepository = moduleRepository;
    }

    public async Task<List<Module>?> GetAllModuleAsync()
    {
        try
        {
            return await _moduleRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<Module?> GetModuleAsync(Int64 id)
    {
        try
        {
            var modules = await _moduleRepository.FindAsync(_ => _.Id == id);

            if (modules != null && modules.Any())
            {
                return modules.FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<bool> CreateModuleAsync(Module module)
    {
        try
        {
            if (module != null)
            {
                return await _moduleRepository.InsertAsync(module);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> UpdateModuleAsync(Module module)
    {
        try
        {
            return await _moduleRepository.UpdateAsync(module);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }       
    
    public async Task<bool> DeleteModuleAsync(Module module)
    {
        try
        {
            return await _moduleRepository.DeleteAsync(module);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }       
}