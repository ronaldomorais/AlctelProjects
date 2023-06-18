using LoggerAppAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerAppAnalyser.Interfaces
{
    public interface IAgenteEventoService
    {
        AgenteEventoLista GetAgenteEventos();
        AgenteEventoLista GetAgenteEventosFilter(DateTime inicio, DateTime fim, int skillDbid = 0);
    }
}
