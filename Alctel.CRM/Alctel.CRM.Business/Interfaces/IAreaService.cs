using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.Context.InMemory.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface IAreaService
{
    Task<List<Area>?> GetAllAreaAsync();
    Task<Area?> GetAreaAsync(Int64 Id);
    Task<bool> CreateAreaAsync(Area data);
    Task<bool> UpdateAreaAsync(Area data);
    Task<bool> DeleteAreaAsync(Area data);

    Task<List<AreaAPI>> GetAreaListAPIAsync();
    Task<List<AreaAPI>> GetAreaActivatedListAPIAsync();
    Task<AreaAPI> GetAreaAPIAsync(Int64 id);
    Task<bool> InsertAreaAPIAsync(AreaAPI AreaAPI);
    Task<bool> UpdateAreaAPIAsync(AreaAPI AreaAPI);
    Task<List<AreaAPI>> SearchAreaAPIAsync(string filter, string value);
}
