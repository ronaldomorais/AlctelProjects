using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Alctel.CRM.Business.Interfaces;

namespace Alctel.CRM.Business.Services;

public class LoginService : ILoginService
{
    private readonly ILoginAPIRepository _loginAPIRepository;

    public LoginService(ILoginAPIRepository loginAPIRepository)
    {
        _loginAPIRepository = loginAPIRepository;
    }

    public async Task<LoginAPI> GetLoginPIAsync(string login)
    {
        try
        {
            var apiResponse = await _loginAPIRepository.GetLoginPIAsync(login);

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

        return new LoginAPI();
    }
}
