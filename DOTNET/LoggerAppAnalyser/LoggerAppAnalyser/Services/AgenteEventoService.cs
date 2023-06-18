using LoggerAppAnalyser.Interfaces;
using LoggerAppAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoggerAppAnalyser.Services
{
    public class AgenteEventoService : IAgenteEventoService
    {
        private readonly IFileFolderManagerService _fileFolderManagerService;

        public AgenteEventoService(IFileFolderManagerService fileFolderManagerService)
        {
            _fileFolderManagerService = fileFolderManagerService;
        }

        private FileDataList GetArquivosLog()
        {
            const string dir = "C:\\Temp\\LoggerApp";
            FileDataList fileDatas = new FileDataList();

            var arquivos = _fileFolderManagerService.GetFiles(dir);

            if (arquivos.Count > 0)
            {
                try
                {
                    fileDatas.AddRange(arquivos.Where(a => a.FileName.EndsWith(".log")).ToList());
                }
                catch (Exception ex)
                { }
            }

            return fileDatas;
        }

        public AgenteEventoLista GetAgenteEventos()
        {
            var arquivosLog = GetArquivosLog();
            AgenteEventoLista agenteEventoLista = new AgenteEventoLista();

            if (arquivosLog.Count > 0)
            {
                foreach (var arquivo in arquivosLog)
                {
                    try
                    {
                        if (!arquivo.FileName.Contains("cti"))
                            continue;

                        StreamReader sr = new StreamReader(arquivo.FullFileName);
                        string? data;
                        while ((data = sr.ReadLine()) != null)
                        {
                            string jsonData = data.Split("=>")[1].Trim();
                            AgenteEvento? agenteEvento = JsonSerializer.Deserialize<AgenteEvento>(jsonData);
                            if (agenteEvento != null)
                                agenteEventoLista.Add(agenteEvento);
                        }
                        sr.Close();
                    }
                    catch (Exception ex)
                    { }
                }
            }

            return agenteEventoLista;
        }

        public AgenteEventoLista GetAgenteEventosFilter(DateTime inicio, DateTime fim, int skillDbid = 0)
        {
            AgenteEventoLista agenteEventosLista = new AgenteEventoLista();
            var agenteEventos = GetAgenteEventos();

            try
            {
                var agenteEventosIntervalo = agenteEventos.Where(a => DateTime.Compare(a.horario, inicio) > 0 && DateTime.Compare(a.horario, fim) < 0).ToList();

                foreach (var agEv in agenteEventosIntervalo)
                {
                    var listaChamadas = agEv.listaChamadas;
                    var listaAgentes = agEv.listaAgentes;

                    AgenteEvento ae = new AgenteEvento()
                    {
                        horario = agEv.horario,
                        master = agEv.master,
                        qtdChamadas = agEv.qtdChamadas,
                        qtdAgentes = agEv.qtdAgentes,
                        listaChamadas = new Chamadas(),
                        listaAgentes = new AgenteLista()
                    };

                    listaAgentes.ForEach(a => a.lskill.RemoveAll(s => s.skill_dbid != skillDbid));
                    listaAgentes.RemoveAll(a => a.lskill.Count == 0);
                    ae.listaAgentes.AddRange(listaAgentes);

                    var chamadas = from c in listaChamadas
                                   join a in listaAgentes on c.age_dbid equals a.dbid
                                   select c;

                    ae.listaChamadas.AddRange(chamadas);

                    agenteEventosLista.Add(ae);
                }
                               
            }
            catch (Exception ex)
            { }
            return agenteEventosLista;
        }

    }
}
