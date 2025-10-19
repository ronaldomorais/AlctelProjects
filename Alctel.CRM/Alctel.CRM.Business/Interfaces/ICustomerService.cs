using Alctel.CRM.API.Entities;
using Alctel.CRM.Business.Model;
using Alctel.CRM.Context.InMemory.Entities;

namespace Alctel.CRM.Business.Interfaces;

public interface ICustomerService
{
    Task<List<Customer>?> GetAllCustomersAsync();
    Task<List<CustomerAPI>> GetPaginatedCustomerAPIAsync(int page, int sizepage);
    Task<int> GetCustomerCountAPIAsync();
    Task<Customer?> GetCustomerAsync(Int64 Id);
    Task<bool> CreateCustomerAsync(Customer customer);
    Task<bool> UpdateCustomerAsync(Customer customer);
    Task<bool> DeleteCustomerAsync(Customer customer);

    Task<List<CustomerAPI>> GetAllCustomerAPIAsync();
    Task<CustomerAPI> GetCustomerAPIAsync(Int64 id);
    Task<CustomerAPI> GetCustomerAPIAsync(string data);
    Task<ResponseServiceModel> InsertCustomerAPIAsync(CustomerCreateAPI customerCreateAPI);
    Task<bool> UpdateCustomerAPIAsync(CustomerAPI customer);
    Task<List<CustomerAPI>> SearchCustomerAPIAsync(string filter, string value);
}