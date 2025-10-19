using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alctel.CRM.API.Entities;
using Alctel.CRM.API.Interfaces;
using Alctel.CRM.Business.Interfaces;
using Alctel.CRM.Context.InMemory.Interfaces;
using Newtonsoft.Json;

namespace Alctel.CRM.Business.Services;

public class LogControllerService : ILogControllerService
{
    private readonly ILogControllerRepository _logControllerRepository;

    public LogControllerService(ILogControllerRepository logControllerRepository)
    {
        _logControllerRepository = logControllerRepository;
    }

    public async Task<bool> InsertLogAPIAsync(LogController logController)
    {
        try
        {
            dynamic obj = new ExpandoObject();
            obj.idObjeto = logController.Id;
            obj.modulo = logController.Module;
            obj.secao = logController.Section == null ? "" : logController.Section;
            obj.campo = logController.Field == null ? "" : logController.Field;
            obj.valor = logController.Value == null ? "" : logController.Value;
            obj.acao = logController.Action == null ? "" : logController.Action;
            obj.idUsuario = logController.UserId;
            obj.descricao = logController.Description;

            var json = JsonConvert.SerializeObject(obj);

            var ret = await _logControllerRepository.InsertDataAsync(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}. Trace: {ex.StackTrace}");
        }

        return false;
    }
}
