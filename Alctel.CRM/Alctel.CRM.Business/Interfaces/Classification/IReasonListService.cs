using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.Context.InMemory.Entities.Classification;


namespace Alctel.CRM.Business.Interfaces.Reason;

public interface IReasonListService
{
    Task<List<ReasonList>?> GetAllReasonListAsync();
    Task<ReasonList?> GetReasonListAsync(Int64 Id);
    Task<bool> CreateReasonListAsync(ReasonList data);
    Task<bool> UpdateReasonListAsync(ReasonList data);
    Task<bool> DeleteReasonListAsync(ReasonList data);

    Task<List<ReasonListAPI>> GetReasonListListAPIAsync();
    Task<List<ReasonListAPI>> GetReasonListActivatedListAPIAsync();
    Task<ReasonListAPI> GetReasonListAPIAsync(Int64 id);
    Task<bool> InsertReasonListAPIAsync(ReasonListAPI ReasonListAPI);
    Task<bool> UpdateReasonListAPIAsync(ReasonListAPI ReasonListAPI);
    Task<List<ReasonListAPI>> SearchReasonListAPIAsync(string filter, string value);
}
