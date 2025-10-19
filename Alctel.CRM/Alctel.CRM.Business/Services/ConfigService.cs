using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.Business.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Alctel.CRM.Business.Services;

public class ConfigService : IConfigService
{
    private readonly IConfiguration _configuration;

    public ConfigService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetGenesysClientId()
    {
        string? genesysClientId = string.Empty;
        try
        {
            genesysClientId = _configuration.GetSection("MiddlewareTicketControl:GenesysClientId").Value;
        }
        catch (Exception ex) { }

        return genesysClientId == null ? string.Empty : genesysClientId;
    }

    public string GetBaseUrl(string physicalPath)
    {
        string baseUrl = string.Empty;        
        string fileSiteJs = $"{physicalPath}\\js\\site.js";

        using (StreamReader reader = new StreamReader(fileSiteJs))
        {
            string? line = null;
            
            while ((line = reader.ReadLine()) != null && line.Contains("const origin_url"))
            {
                baseUrl = line.Replace("const origin_url = ", "");
                baseUrl = baseUrl.Replace("const origin_url=", "");
                baseUrl = baseUrl.Replace("'", "");
                baseUrl = baseUrl.Replace("\"", "");
                break;
            }            
        }

        return baseUrl;
    }
}
