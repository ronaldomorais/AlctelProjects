using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface IClassificationListItemsAPIRepository
{
    Task<APIResponse<List<ClassificationListItemsAPI>>> GetClassificationListItemsAPIAsync(Int64 classificationListId);
    Task<APIResponse<List<ClassificationListItemsAPI>>> GetClassificationListItemsActiveAPIAsync(Int64 classificationListId);
    Task<APIResponse<ClassificationListItemsAPI>> GetClassificationListItemAPIAsync(Int64 id);
    Task<APIResponse<bool>> UpdateClassificationItemsAPIAsync(Int64 id, bool status);
    Task<APIResponse<int>> InsertClassificationItemsAPIAsync(string data);
}
