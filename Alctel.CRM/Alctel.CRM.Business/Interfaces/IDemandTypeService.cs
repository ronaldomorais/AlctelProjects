using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.Context.InMemory.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface IDemandTypeService
{
    Task<List<DemandType>?> GetAllDemandTypeAsync();
    Task<DemandType?> GetDemandTypeAsync(Int64 Id);
    Task<bool> CreateDemandTypeAsync(DemandType data);
    Task<bool> UpdateDemandTypeAsync(DemandType data);
    Task<bool> DeleteDemandTypeAsync(DemandType data);

    Task<List<DemandTypeAPI>> GetDemandTypeListAPIAsync();
    Task<List<DemandTypeAPI>> GetDemandTypeActivatedListAPIAsync();
    Task<DemandTypeAPI> GetDemandTypeAPIAsync(Int64 id);
    Task<bool> InsertDemandTypeAPIAsync(DemandTypeAPI DemandTypeAPI);
    Task<bool> UpdateDemandTypeAPIAsync(DemandTypeAPI DemandTypeAPI);
    Task<List<DemandTypeAPI>> SearchDemandTypeAPIAsync(string filter, string value);
}
