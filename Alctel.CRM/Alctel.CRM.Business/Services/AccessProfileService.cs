using System.Collections.Generic;
using System.Net.Http.Headers;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Alctel.CRM.API.Repositories;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Alctel.CRM.Business.Services;

public class AccessProfileService : IAccessProfileService
{
    private readonly IAccessProfileRepository _accessProfileRepository;
    private readonly IModuleService _moduleService;
    private readonly IActionPermissionService _actionPermissionService;
    private readonly IAccessProfileAPIRepository _accessProfileAPIRepository;

    public AccessProfileService(IAccessProfileRepository accessProfileRepository, IModuleService moduleService, IActionPermissionService actionPermissionService, IAccessProfileAPIRepository accessProfileAPIRepository)
    {
        _accessProfileRepository = accessProfileRepository;
        _moduleService = moduleService;
        _actionPermissionService = actionPermissionService;
        _accessProfileAPIRepository = accessProfileAPIRepository;
    }

    public async Task<List<AccessProfile>?> GetAllAccessProfileAsync()
    {
        try
        {
            return await _accessProfileRepository.GetAllAsync(_ => _.Include(_ => _.Modules).ThenInclude(_ => _.ActionPermissions));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<AccessProfile?> GetAccessProfileAsync(Int64 id)
    {
        try
        {
            var accessProfiles = await _accessProfileRepository.FindAsync(_ => _.Id == id, _ => _.Include(_ => _.Modules).ThenInclude(_ => _.ActionPermissions));

            if (accessProfiles != null && accessProfiles.Any())
            {
                var accessProfile = accessProfiles.FirstOrDefault();
                var modules = await _moduleService.GetAllModuleAsync();
                var actionpermissions = await _actionPermissionService.GetAllActionPermissionAsync();

                if (accessProfile != null)
                {
                    AccessProfile accessProfilesResult = new AccessProfile();

                    accessProfilesResult.Id = accessProfile.Id;
                    accessProfilesResult.Name = accessProfile.Name;
                    accessProfilesResult.Description = accessProfile.Description;
                    accessProfilesResult.Modules = new List<Module>();

                    if (modules != null && modules.Any())
                    {
                        foreach (var module in modules)
                        {
                            var moduleFromDB = accessProfile.Modules.Where(_ => _.Id == module.Id).FirstOrDefault();

                            Module m = new Module();
                            m.Id = module.Id;
                            m.Name = module.Name;
                            m.Description = module.Description;
                            m.ActionPermissions = new List<ActionPermission>();

                            if (moduleFromDB != null)
                            {
                                m.ModuleChecked = true;

                                if (actionpermissions != null)
                                {
                                    foreach (var action in actionpermissions)
                                    {
                                        var actionsFromDB = moduleFromDB.ActionPermissions.Where(_ => _.Id == action.Id).FirstOrDefault();

                                        ActionPermission a = new ActionPermission();
                                        a.Id = action.Id;
                                        a.Name = action.Name;
                                        a.Description = action.Description;

                                        if (actionsFromDB != null)
                                        {
                                            a.ActionChecked = true;
                                        }
                                        else
                                        {
                                            a.ActionChecked = false;
                                        }

                                        m.ActionPermissions.Add(a);
                                    }
                                }
                            }
                            else
                            {
                                m.ModuleChecked = false;

                                if (actionpermissions != null)
                                {
                                    foreach (var action in actionpermissions)
                                    {
                                        ActionPermission a = new ActionPermission();
                                        a.Id = action.Id;
                                        a.Name = action.Name;
                                        a.Description = action.Description;
                                        a.ActionChecked = false;

                                        m.ActionPermissions.Add(a);
                                    }
                                }
                            }

                            accessProfilesResult.Modules.Add(m);
                        }
                    }

                    return accessProfilesResult;
                }



                //var accessprofile = accessProfiles.FirstOrDefault();

                //if (accessprofile != null)
                //{
                //    var modules = await _moduleService.GetAllModuleAsync();
                //    var actionpermissions = await _actionPermissionService.GetAllActionPermissionAsync();

                //    if (modules != null)
                //    {
                //        foreach (var module in modules)
                //        {
                //            if (accessprofile.Modules.Any(_ => _.Id == module.Id) == false)
                //            {
                //                Module m = new Module();
                //                m.Id = module.Id;
                //                m.Name = module.Name;
                //                m.Description = module.Description;
                //                m.ModuleChecked = false;
                //                m.ModuleChecked = false;

                //                if (actionpermissions != null)
                //                {
                //                    var actions = actionpermissions.Select(a => { a.ActionChecked = false; return a; });
                //                    m.ActionPermissions = new List<ActionPermission>();
                //                    m.ActionPermissions.AddRange(actions);
                //                    //var actionUpdated = actions.Select(a => { a.ActionChecked = false; return a; }).ToList();
                //                    //module.ActionPermissions = new List<ActionPermission>();
                //                    //module.ActionPermissions.AddRange(actionUpdated);
                //                }

                //                accessprofile.Modules.Add(m);
                //            }
                //        }

                //        //var actionpermissions = await _actionPermissionService.GetAllActionPermissionAsync();

                //        //if (actionpermissions != null)
                //        //{
                //        //    foreach (var action in actionpermissions)
                //        //    {
                //        //        for (int i = 0; i < accessprofile.Modules.Count; i++)
                //        //        {
                //        //            if (accessprofile.Modules[i].ActionPermissions.Any(_ => _.Id == action.Id) == false)
                //        //            {
                //        //                action.ActionChecked = false;
                //        //                accessprofile.Modules[i].ActionPermissions.Add(action);
                //        //            }
                //        //        }
                //        //    }
                //        //}
                //    }
                //}

                //return accessprofile;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<bool> CreateAccessProfileAsync(AccessProfile accessProfile)
    {
        try
        {
            if (accessProfile != null)
            {
                return await _accessProfileRepository.InsertAsync(accessProfile);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> UpdateAccessProfileAsync(AccessProfile accessProfile)
    {
        try
        {
            var profile = new AccessProfile();
            profile.Id = accessProfile.Id;
            profile.Name = accessProfile.Name;
            profile.Description = accessProfile.Description;

            if (accessProfile.Modules != null && accessProfile.Modules.Any())
            {
                profile.Modules = new List<Module>();

                var moduleSelected = accessProfile.Modules.Where(_ => _.ModuleChecked).ToList();

                if (moduleSelected != null && moduleSelected.Any())
                {
                    foreach (var module in moduleSelected)
                    {
                        Module m = new Module();
                        m.Id = module.Id;
                        m.Name = module.Name;
                        m.Description = module.Description;
                        m.ModuleChecked = module.ModuleChecked;
                        m.ActionPermissions = new List<ActionPermission>();

                        var actionsSelected = module.ActionPermissions.Where(_ => _.ActionChecked).ToList();

                        if (actionsSelected != null && actionsSelected.Any())
                        {
                            foreach (var action in actionsSelected)
                            {
                                ActionPermission a = new ActionPermission();
                                a.Id = action.Id;
                                a.Name = action.Name;
                                a.Description = action.Description;
                                a.ActionChecked = action.ActionChecked;

                                m.ActionPermissions.Add(a);
                            }
                        }

                        profile.Modules.Add(m);
                    }
                }               
            }
            //return await _accessProfileRepository.UpdateAsync(profile);
            return await _accessProfileRepository.DeleteInsertAsync(profile.Id, profile);



        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> DeleteAccessProfileAsync(AccessProfile accessProfile)
    {
        try
        {
            return await _accessProfileRepository.DeleteAsync(accessProfile);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }


    public async Task<List<AccessProfileAPI>> GetAllAccessProfileAPIAsync()
    {
        try
        {
            var apiResponse = await _accessProfileAPIRepository.GetAllDataAsync();

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                    return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<AccessProfileAPI>();
    }

    public async Task<List<AccessProfileAPI>> GetAccessProfileActivatedListAPIAsync()
    {
        try
        {
            var apiResponse = await _accessProfileAPIRepository.GetAccessProfileActivatedListAPIAsync();

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                    return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<AccessProfileAPI>();
    }
}