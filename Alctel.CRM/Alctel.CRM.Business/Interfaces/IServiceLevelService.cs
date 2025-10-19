using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.Context.InMemory.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface IServiceLevelService
{
    Task<List<ServiceLevel>?> GetAllServiceLevelAsync();
    Task<ServiceLevel?> GetServiceLevelAsync(Int64 Id);
    Task<bool> CreateServiceLevelAsync(ServiceLevel data);
    Task<bool> UpdateServiceLevelAsync(ServiceLevel data);
    Task<bool> DeleteServiceLevelAsync(ServiceLevel data);

    Task<List<ServiceLevelAPI>> GetServiceLevelListAPIAsync();
    Task<List<ServiceLevelAPI>> GetServiceLevelActivatedListAPIAsync();
    Task<ServiceLevelAPI> GetServiceLevelAPIAsync(Int64 id);
    Task<bool> InsertServiceLevelAPIAsync(ServiceLevelAPI ServiceLevelAPI);
    Task<bool> UpdateServiceLevelAPIAsync(ServiceLevelAPI ServiceLevelAPI);
    Task<List<ServiceLevelAPI>> SearchServiceLevelAPIAsync(string filter, string value);

}
