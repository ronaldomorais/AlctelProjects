using LoggerAppAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerAppAnalyser.Interfaces
{
    public interface IFilaEventosService
    {
        FilaEventoLista GetFilaEventos();
        FilaEventoLista GetFilaEventosFilter(string connid);
    }
}
