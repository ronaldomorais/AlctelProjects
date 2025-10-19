using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface IClassificationDemandTypeAPIRepository
{
    Task<APIResponse<List<ClassificationDemandTypeAPI>>> GetClassificationDemandTypeListAPIAsync();
    Task<APIResponse<List<ClassificationDemandTypeAPI>>> GetClassificationDemandTypeActivatedListAPIAsync();
    Task<APIResponse<List<ClassificationDemandTypeAPI>>> GetClassificationDemandTypeListAPIAsync(Int64 id);
    Task<APIResponse<bool>> InsertClassificationDemandTypeAPIAsync(string data);
    Task<APIResponse<bool>> UpdateClassificationDemandTypeAPIAsync(Int64 id, bool status);
    Task<APIResponse<List<ClassificationDemandTypeAPI>>> SearchDataAsync(string data);
}
