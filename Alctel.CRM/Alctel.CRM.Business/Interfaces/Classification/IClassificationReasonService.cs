using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface IClassificationReasonService
{
    Task<List<ClassificationReasonAPI>> GetClassificationReasonListAPIAsync();
    Task<List<ClassificationReasonAPI>> GetClassificationReasonActivatedListAPIAsync();
    Task<List<ClassificationReasonAPI>> GetClassificationReasonAPIAsync(Int64 id);
    Task<bool> InsertClassificationReasonAPIAsync(ClassificationReasonAPI ClassificationReasonAPI);
    Task<bool> UpdateClassificationReasonAPIAsync(ClassificationReasonAPI ClassificationReasonAPI);
    Task<List<ClassificationReasonAPI>> SearchClassificationReasonAPIAsync(string filter, string value);
    Task<List<ClassificationReasonChildrenAPI>> GetClassificationReasonListChildrenAPIAsync(Int64 reasonid);
}
