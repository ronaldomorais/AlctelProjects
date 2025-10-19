using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface IClassificationDemandService
{
    Task<List<ClassificationDemandAPI>> GetClassficationDemandListAPIAsync();
    Task<List<ClassificationDemandAPI>> GetClassficationDemandActivatedListAPIAsync();
    Task<ClassificationDemandAPI> GetClassficationDemandAPIAsync(Int64 id);
    Task<bool> InsertClassficationDemandAPIAsync(ClassificationDemandAPI ClassficationDemandAPI);
    Task<bool> UpdateClassficationDemandAPIAsync(ClassificationDemandAPI ClassficationDemandAPI);
    Task<List<ClassificationDemandAPI>> SearchClassficationDemandAPIAsync(string filter, string value);

}
