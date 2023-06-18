using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerAppAnalyser.Models
{
    public class FilaEvento
    {
        public DateTime horario { get; set; } = DateTime.Now;
        public bool master { get; set; }
        public int qtd { get; set; }
        public FilaLista lista { get; set; } = new FilaLista();
    }

    public class FilaEventoLista : List<FilaEvento> { }

    public class FilaInfo
    {
        public DateTime inicio { get; set; }
        public DateTime inFila { get; set; }
        public string fila { get; set; } = string.Empty;
        public string origem { get; set; } = string.Empty;
        public string connid { get; set; } = string.Empty;
        public bool ativa { get; set; }
    }

    public class FilaLista : List<FilaInfo> { }
}
