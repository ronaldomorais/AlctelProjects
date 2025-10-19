using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.Context.InMemory.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface IServiceUnitService
{
    Task<List<ServiceUnit>?> GetAllServiceUnitAsync();
    Task<ServiceUnit?> GetServiceUnitAsync(Int64 Id);
    Task<bool> CreateServiceUnitAsync(ServiceUnit data);
    Task<bool> UpdateServiceUnitAsync(ServiceUnit data);
    Task<bool> DeleteServiceUnitAsync(ServiceUnit data);

    Task<List<ServiceUnitAPI>> GetServiceUnitListAPIAsync();
    Task<List<ServiceUnitAPI>> GetServiceUnitActivatedListAPIAsync();
    Task<ServiceUnitAPI> GetServiceUnitAPIAsync(Int64 id);
    Task<bool> InsertServiceUnitAPIAsync(ServiceUnitAPI serviceUnitAPI);
    Task<bool> UpdateServiceUnitAPIAsync(ServiceUnitAPI serviceUnitAPI);
    Task<List<ServiceUnitAPI>> SearchServiceUnitAPIAsync(string filter, string value);
}
