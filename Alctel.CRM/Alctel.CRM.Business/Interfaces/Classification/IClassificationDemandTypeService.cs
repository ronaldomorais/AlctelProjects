using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface IClassificationDemandTypeService
{
    Task<List<ClassificationDemandTypeAPI>> GetClassificationDemandTypeListAPIAsync();
    Task<List<ClassificationDemandTypeAPI>> GetClassificationDemandTypeActivatedListAPIAsync();
    Task<List<ClassificationDemandTypeAPI>> GetClassificationDemandTypeListAPIAsync(Int64 id);
    Task<bool> InsertClassificationDemandTypeAPIAsync(ClassificationDemandTypeAPI ClassificationDemandTypeAPI);
    Task<bool> UpdateClassificationDemandTypeAPIAsync(ClassificationDemandTypeAPI ClassificationDemandTypeAPI);
    Task<List<ClassificationDemandTypeAPI>> SearchClassificationDemandTypeAPIAsync(string filter, string value);
}
