using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;

namespace Alctel.CRM.API.Interfaces;

public interface ICustomerAPIRepository
{
    Task<APIResponse<List<CustomerAPI>>> GetAllDataAsync();
    Task<APIResponse<List<CustomerAPI>>> GetPagenatedDataAsync(int page = 1, int sizepage = 50);
    Task<APIResponse<int>> GetDataAsync();
    Task<APIResponse<CustomerAPI>> GetDataAsync(Int64 id);
    Task<APIResponse<CustomerAPI>> GetDataAsync(string data, string endpoint);
    Task<APIResponse<bool>> UpdateDataAsync(string data);
    Task<APIResponse<string>> InsertCustomerAPIAsync(string data);
    Task<APIResponse<List<CustomerAPI>>> SearchDataAsync(string data);
}
