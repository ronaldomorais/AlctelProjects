using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface IReasonListAPIRepository
{
    Task<APIResponse<List<ReasonListAPI>>> GetReasonListListAPIAsync();
    Task<APIResponse<List<ReasonListAPI>>> GetReasonListActivatedListAPIAsync();
    Task<APIResponse<ReasonListAPI>> GetReasonListAPIAsync(Int64 id);
    Task<APIResponse<bool>> InsertReasonListAPIAsync(string data);
    Task<APIResponse<bool>> UpdateReasonListAPIAsync(Int64 id, bool status);
    Task<APIResponse<List<ReasonListAPI>>> SearchDataAsync(string data);
}
