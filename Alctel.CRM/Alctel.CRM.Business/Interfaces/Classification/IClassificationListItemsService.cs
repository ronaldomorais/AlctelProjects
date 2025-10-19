using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.Business.Interfaces.Classification;

public interface IClassificationListItemsService
{
    Task<List<ClassificationListItemsAPI>> GetClassificationListItemsListAPIAsync(Int64 classificationListId);
    Task<ClassificationListItemsAPI> GetClassificationListItemAPIAsync(Int64 classificationListId);
    Task<List<ClassificationListItemsAPI>> GetClassificationListItemsActiveAPIAsync(Int64 classificationListId);
    Task<bool> UpdateClassificationListItemsAPIAsync(ClassificationListItemsAPI classificationListItemsAPI);
    Task<int> InsertClassificationListItemsAPIAsync(ClassificationListItemsAPI classificationListItemsAPI);
}
