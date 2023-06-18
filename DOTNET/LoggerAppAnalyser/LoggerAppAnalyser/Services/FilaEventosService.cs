using LoggerAppAnalyser.Interfaces;
using LoggerAppAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LoggerAppAnalyser.Services
{
    public class FilaEventosService : IFilaEventosService
    {
        private readonly IFileFolderManagerService _fileFolderManagerService;
        public FilaEventosService(IFileFolderManagerService fileFolderManagerService)
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

        public FilaEventoLista GetFilaEventos()
        {
            var arquivosLog = GetArquivosLog();
            FilaEventoLista filaEventoLista = new FilaEventoLista();

            if (arquivosLog.Count > 0)
            {
                foreach (var arquivo in arquivosLog)
                {
                    try
                    {
                        if (!arquivo.FileName.Contains("Fila"))
                            continue;

                        StreamReader sr = new StreamReader(arquivo.FullFileName);
                        string? data;
                        while ((data = sr.ReadLine()) != null)
                        {
                            string jsonData = data.Split("=>")[1].Trim();
                            FilaEvento? filaEvento = JsonSerializer.Deserialize<FilaEvento>(jsonData);

                            if (filaEvento != null)
                            {
                                if (filaEvento.lista != null)
                                {
                                    if (filaEvento.lista.Count > 0)
                                    {
                                        filaEventoLista.Add(filaEvento);
                                    }
                                }
                            }
                        }
                        sr.Close();
                    }
                    catch (Exception ex)
                    { }
                }
            }

            return filaEventoLista;
        }

        public FilaEventoLista GetFilaEventosFilter(string connid)
        {
            var filaEventos = GetFilaEventos();
            FilaEventoLista filaEventosLista = new FilaEventoLista();
            try
            {
                filaEventosLista.AddRange(filaEventos.Where(f => f.lista.All(l => l.connid == connid)).ToList());
            }
            catch (Exception ex)
            { }
            return filaEventosLista;
        }
    }
}
