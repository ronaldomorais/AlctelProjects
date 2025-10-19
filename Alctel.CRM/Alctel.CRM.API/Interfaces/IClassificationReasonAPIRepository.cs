using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface IClassificationReasonAPIRepository
{
    Task<APIResponse<List<ClassificationReasonAPI>>> GetClassificationReasonListAPIAsync();
    Task<APIResponse<List<ClassificationReasonAPI>>> GetClassificationReasonActivatedListAPIAsync();
    Task<APIResponse<List<ClassificationReasonAPI>>> GetClassificationReasonAPIAsync(Int64 id);
    Task<APIResponse<bool>> InsertClassificationReasonAPIAsync(string data);
    Task<APIResponse<bool>> UpdateClassificationReasonAPIAsync(Int64 id, bool status);
    Task<APIResponse<List<ClassificationReasonAPI>>> SearchDataAsync(string data);
    Task<APIResponse<List<ClassificationReasonChildrenAPI>>> GetClassificationReasonListChildrenAPIAsync(Int64 reasondid);
}
