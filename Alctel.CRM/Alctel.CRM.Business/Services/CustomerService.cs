using System;
using System.Dynamic;
using System.Text;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Alctel.CRM.API.Repositories;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Business.Model;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Interfaces;
using Newtonsoft.Json;

namespace Alctel.CRM.Business.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerAPIRepository _customerAPIRepository;
    public CustomerService(ICustomerRepository customerRepository, ICustomerAPIRepository customerAPIRepository)
    {
        _customerRepository = customerRepository;
        _customerAPIRepository = customerAPIRepository;
    }

    public async Task<List<Customer>?> GetAllCustomersAsync()
    {
        try
        {
            return await _customerRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<Customer?> GetCustomerAsync(Int64 id)
    {
        try
        {
            var customers = await _customerRepository.FindAsync(_ => _.Id == id);

            if (customers != null && customers.Any())
            {
                return customers.FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return null;
    }

    public async Task<bool> CreateCustomerAsync(Customer customer)
    {
        try
        {
            if (customer != null)
            {
                Random random = new Random();
                customer.Id = random.NextInt64(1000000000000L);
                customer.CreatedOn = DateTime.Now;
                customer.LastControlLog = $"{DateTime.Now.ToString("dd/mm/yyyy")};{DateTime.Now.ToString("HH:mm")};Usu�rio Respons�vel;Cliente,Cadastro;Nome,Nome Social,CPF,Email 1,Email 2,Telefone,WhatsApp,CNPJ,Nome da Empresa,Telefone 1,Telefone 2,Categoria,SubCategoria,Matr�cula;{customer.FirstName} {customer.LastName},{customer.SocialAffectionateName},{customer.Cpf},{customer.Email1},{customer.Email2},{customer.PhoneNumber},{customer.MobilePhone},{customer.Cnpj},{customer.CompanyName},{customer.PhoneNumber1},{customer.PhoneNumber2},{customer.Category},{customer.SubCategory},{customer.Registration}";
                return await _customerRepository.InsertAsync(customer);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> UpdateCustomerAsync(Customer customer)
    {
        try
        {
            //var customerFromDB = await _customerRepository.FindAsync(_ => _.Id == customer.Id);

            //if (customerFromDB != null)
            //{
            //    StringBuilder lastLogControl = new StringBuilder();
            //    lastLogControl.Append($"{DateTime.Now.ToString("dd/MM/yyyy")}");
            //    lastLogControl.Append($";{DateTime.Now.ToString("HH:mm")}");
            //    lastLogControl.Append($";Usu�rio Responsavel");
            //    lastLogControl.Append($";Cliente");
            //    lastLogControl.Append($";Cadastro");

            //    var customerToCompare = customerFromDB.FirstOrDefault();
            //    if (customerToCompare != null)
            //    {
            //        if (customerToCompare.FirstName != customer.FirstName || customerToCompare.LastName != customer.LastName)
            //        {
            //            lastLogControl.Append("Nome");
            //            lastLogControl.Append($"{customer.FirstName} {customer.LastName}");
            //        }
            //    }
            //}

            return await _customerRepository.UpdateAsync(customer);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<bool> DeleteCustomerAsync(Customer customer)
    {
        try
        {
            return await _customerRepository.DeleteAsync(customer);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }


    public async Task<List<CustomerAPI>> GetAllCustomerAPIAsync()
    {
        try
        {
            var apiResponse = await _customerAPIRepository.GetAllDataAsync();

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                    return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<CustomerAPI>();
    }

    public async Task<List<CustomerAPI>> GetPaginatedCustomerAPIAsync(int page, int sizepage)
    {
        try
        {
            var apiResponse = await _customerAPIRepository.GetPagenatedDataAsync(page, sizepage);

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                    return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<CustomerAPI>();
    }

    public async Task<int> GetCustomerCountAPIAsync()
    {
        try
        {
            var apiResponse = await _customerAPIRepository.GetDataAsync();

            if (apiResponse.IsSuccessStatusCode)
            {
                return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new int();
    }
    public async Task<CustomerAPI> GetCustomerAPIAsync(Int64 id)
    {
        try
        {
            var apiResponse = await _customerAPIRepository.GetDataAsync(id);

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                    return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new CustomerAPI();
    }

    public async Task<CustomerAPI> GetCustomerAPIAsync(string data)
    {
        try
        {
            string path = "customerdata";

            if (data.Contains("@"))
            {
                path = "customerdataemail";
            }

            var apiResponse = await _customerAPIRepository.GetDataAsync(data, path);

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                    return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new CustomerAPI();
    }    

    public async Task<ResponseServiceModel> InsertCustomerAPIAsync(CustomerCreateAPI customerCreateAPI)
    {
        ResponseServiceModel responseServiceModel = new ResponseServiceModel();
        try
        {
            if (customerCreateAPI.Cpf != null)
            {
                customerCreateAPI.Cpf = string.Join("", customerCreateAPI.Cpf.Where(char.IsDigit).ToList());
            }

            if (customerCreateAPI.Cnpj != null)
            {
                customerCreateAPI.Cnpj = string.Join("", customerCreateAPI.Cnpj.Where(char.IsDigit).ToList());
            }

            if (customerCreateAPI.PhoneNumber1 != null)
            {
                customerCreateAPI.PhoneNumber1 = string.Join("", customerCreateAPI.PhoneNumber1.Where(char.IsDigit).ToList());
            }

            if (customerCreateAPI.PhoneNumber2 != null)
            {
                customerCreateAPI.PhoneNumber2 = string.Join("", customerCreateAPI.PhoneNumber2.Where(char.IsDigit).ToList());
            }

            if (customerCreateAPI.PhoneNumberCompany != null)
            {
                customerCreateAPI.PhoneNumberCompany = string.Join("", customerCreateAPI.PhoneNumberCompany.Where(char.IsDigit).ToList());
            }

            if (customerCreateAPI.Registration != null)
            {
                customerCreateAPI.Registration = string.Join("", customerCreateAPI.Registration.Where(char.IsDigit).ToList());
            }

            customerCreateAPI.FirstName = string.Concat(customerCreateAPI.FirstName, " ", customerCreateAPI.LastName);
            string json = JsonConvert.SerializeObject(customerCreateAPI, Formatting.Indented);

            var apiResponse = await _customerAPIRepository.InsertCustomerAPIAsync(json);

            responseServiceModel.IsValid = apiResponse.IsSuccessStatusCode;

            if (apiResponse.IsSuccessStatusCode)
            {
                responseServiceModel.Value = apiResponse.Response ?? string.Empty;
            }
            else
            {
                string additionalMessage = apiResponse.AdditionalMessage ?? string.Empty;

                if (additionalMessage.Contains("cliente_cpf_unique"))
                {
                    responseServiceModel.Value = "Duplicidade: Já existe um cliente ativo com este CPF. Não é permitido cadastrar clientes com o mesmo CPF";
                }
                else
                {
                    responseServiceModel.Value = additionalMessage;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return responseServiceModel;
    }

    public async Task<bool> UpdateCustomerAPIAsync(CustomerAPI customer)
    {
        try
        {
            dynamic obj = new ExpandoObject();
            obj.idCliente = customer.Id;
            obj.nomeSocial = customer.SocialAffectionateName;
            obj.telefoneSecundario = customer.PhoneNumber2;
            obj.telefoneEmpresa = customer.PhoneNumberCompany;
            obj.emailSecundario = customer.Email2;

            if (customer.Cnpj != null)
                obj.cnpj = string.Join("", customer.Cnpj.Where(char.IsDigit).ToList());
            else
                obj.cnpj = null;
            obj.nomeEmpresa = customer.CompanyName;
            obj.campo1 = customer.ExtraField01;
            obj.campo2 = customer.ExtraField02;
            obj.campo3 = customer.ExtraField03;

            var json = JsonConvert.SerializeObject(obj);

            var ret = await _customerAPIRepository.UpdateDataAsync(json);

            if (ret != null)
            {
                return ret.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }

    public async Task<List<CustomerAPI>> SearchCustomerAPIAsync(string filter, string value)
    {
        try
        {
            dynamic obj = new ExpandoObject();
            obj.chave = filter;
            obj.valor = value;

            var json = JsonConvert.SerializeObject(obj);

            var apiResponse = await _customerAPIRepository.SearchDataAsync(json);

            if (apiResponse.IsSuccessStatusCode)
            {
                if (apiResponse.Response != null)
                    return apiResponse.Response;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return new List<CustomerAPI>();
    }
}