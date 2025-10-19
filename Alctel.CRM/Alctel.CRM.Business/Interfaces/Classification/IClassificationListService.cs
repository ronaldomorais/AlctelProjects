using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface IClassificationListService
{
    Task<List<ClassificationListAPI>> GetClassificationListListAPIAsync();

    //Task<List<ClassificationListAPI>> GetClassificationListActivatedListAPIAsync();
    //Task<ClassificationListAPI> GetClassificationListAPIAsync(Int64 id);
    //Task<bool> InsertClassificationListAPIAsync(ClassificationListAPI ClassificationListAPI);
    //Task<bool> UpdateClassificationListAPIAsync(ClassificationListAPI ClassificationListAPI);
    //Task<List<ClassificationListAPI>> SearchClassificationListAPIAsync(string filter, string value);
}
