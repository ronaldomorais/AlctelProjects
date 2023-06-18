using LoggerAppAnalyser.Interfaces;
using LoggerAppAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoggerAppAnalyser
{
    public class LoggerAppAnalyserService
    {
        private readonly IFilaEventosService _filaEventosService;
        private readonly IAgenteEventoService _agenteEventoService;
        public LoggerAppAnalyserService(IFilaEventosService filaEventosService, IAgenteEventoService agenteEventoService)
        {
            _filaEventosService = filaEventosService;
            _agenteEventoService = agenteEventoService;
        }

        public void StartAnalyser(string connid)
        {
            int skillDbid = 265;
            var filaEventos = _filaEventosService.GetFilaEventosFilter(connid);

            var filaEventoFirst = filaEventos.FirstOrDefault();
            var filaEventoLast = filaEventos.LastOrDefault();

            try
            {
                if (filaEventoFirst != null && filaEventoLast != null)
                {
                    DateTime inicio = filaEventoFirst.lista[0].inFila;
                    DateTime fim = filaEventoLast.horario;

                    var agenteEventos = _agenteEventoService.GetAgenteEventosFilter(inicio, fim, skillDbid);

                    StreamWriter writer = new StreamWriter("result.log");
                    writer.WriteLine(JsonSerializer.Serialize(filaEventoFirst, new JsonSerializerOptions { WriteIndented = true }));
                    writer.WriteLine("");
                    int counter = 1;
                    foreach (var aEvt in agenteEventos.OrderBy(a => a.horario))
                    {
                        var agenteSkill = (from ag in aEvt.listaAgentes
                                     select new { ageDbid = ag.dbid, skillDbid = ag.lskill[0].skill_dbid, level = ag.lskill[0].level, emPausa = ag.lpausas.FindAll(p => p.pausaativa).Count > 0 ? true : false, emChamada = aEvt.listaChamadas.FindAll(c => c.age_dbid == ag.dbid).Count > 0 ? true : false, connid = aEvt.listaChamadas.Where(c => c.age_dbid == ag.dbid).ToList() }).ToList();

                        int qtdAgentesAptos = agenteSkill.Where(a => a.skillDbid == skillDbid && a.level > 0 && !a.emChamada && !a.emPausa).ToList().Count;

                        writer.WriteLine($"*************   Requisição {counter} Inicio   *************\r\n");
                        writer.WriteLine($"Horário: {aEvt.horario}, Quantidade de agentes com skill {skillDbid}: {aEvt.listaAgentes.Count}, Agentes Disponíveis: {qtdAgentesAptos}.");
                        //writer.WriteLine(JsonSerializer.Serialize(agenteSkill, new JsonSerializerOptions { WriteIndented = true }));
                        //writer.WriteLine("");
                        //writer.WriteLine("Mensagem Original");
                        //writer.WriteLine("");
                        writer.WriteLine(JsonSerializer.Serialize(aEvt, new JsonSerializerOptions { WriteIndented = true }));
                        writer.WriteLine($"*************   Requisição {counter} Término   *************");
                        writer.WriteLine("");
                        counter++;
                    }
                    writer.WriteLine(JsonSerializer.Serialize(filaEventoLast, new JsonSerializerOptions { WriteIndented = true }));
                    writer.Close();
                }
            }
            catch (Exception ex)
            { }
        }
    }
}
