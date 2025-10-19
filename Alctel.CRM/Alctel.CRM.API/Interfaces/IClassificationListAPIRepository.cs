using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface IClassificationListAPIRepository
{
    Task<APIResponse<List<ClassificationListAPI>>> GetClassificationListListAPIAsync();

    //Task<APIResponse<List<ClassificationListAPI>>> GetClassificationListActivatedListAPIAsync();
    //Task<APIResponse<ClassificationListAPI>> GetClassificationListAPIAsync(Int64 id);
    //Task<APIResponse<bool>> InsertClassificationListAPIAsync(string data);
    //Task<APIResponse<bool>> UpdateClassificationListAPIAsync(Int64 id, bool status);
    //Task<APIResponse<List<ClassificationListAPI>>> SearchDataAsync(string data);
}
