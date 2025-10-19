using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface IClassificationDemandAPIRepository
{
    Task<APIResponse<List<ClassificationDemandAPI>>> GetClassficationDemandListAPIAsync();
    Task<APIResponse<List<ClassificationDemandAPI>>> GetClassficationDemandActivatedListAPIAsync();
    Task<APIResponse<ClassificationDemandAPI>> GetClassficationDemandAPIAsync(Int64 id);
    Task<APIResponse<bool>> InsertClassficationDemandAPIAsync(string data);
    Task<APIResponse<bool>> UpdateClassficationDemandAPIAsync(Int64 id, bool status);
    Task<APIResponse<List<ClassificationDemandAPI>>> SearchDataAsync(string data);
}
