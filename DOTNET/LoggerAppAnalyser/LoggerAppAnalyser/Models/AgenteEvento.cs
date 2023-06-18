using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerAppAnalyser.Models
{
    public class AgenteEvento
    {
        public DateTime horario { get; set; }
        public bool master { get; set; }
        public int qtdChamadas { get; set; }
        public int qtdAgentes { get; set; }
        public Chamadas listaChamadas { get; set; } = new Chamadas();
        public AgenteLista listaAgentes { get; set; } = new AgenteLista();

    }

    public class AgenteEventoLista : List<AgenteEvento> { }

    public class Chamada
    {
        public DateTime inicio { get; set; }
        public DateTime fim { get; set; }
        public DateTime inicioRing { get; set; }
        public DateTime fimRing { get; set; }
        public string connid { get; set; } = string.Empty;
        public string connidpausa { get; set; } = string.Empty;
        public string fila { get; set; } = string.Empty;
        public bool ativa { get; set; }
        public bool ativapausa { get; set; }
        public int age_dbid { get; set; }
        public string origem { get; set; } = string.Empty;
        public string evento { get; set; } = string.Empty;
        public int callType { get; set; }
    }

    public class Chamadas : List<Chamada> { }

    public class Agente
    {
        public int dbid { get; set; }
        public string agentid { get; set; } = string.Empty;
        public string place { get; set; } = string.Empty;
        public DateTime horalogin { get; set; }
        public DateTime horalogout { get; set; }
        public bool loginativo { get; set; }
        public bool pausaativa { get; set; }
        public SkillLista lskill { get; set; } = new SkillLista();
        public PausaLista lpausas { get; set; } = new PausaLista();
        public int evt_number { get; set; }
        public int atendidas { get; set; }
        public int discadas { get; set; }
        public int recebidas { get; set; }
        public int tconversacao { get; set; }
        public DateTime ultimachamada { get; set; }
    }

    public class AgenteLista : List<Agente> { }

    public class Skill
    {
        public int dbid { get; set; }
        public string agentid { get; set; } = string.Empty;
        public string skill_nome { get; set; } = string.Empty;
        public int skill_dbid { get; set; }
        public int level { get; set; }
        public DateTime inicio { get; set; }
        public DateTime fim { get; set; }
        public int evt_number { get; set; }
    }

    public class SkillLista : List<Skill> { }

    public class Pausa
    {
        public DateTime inicio { get; set; }
        public DateTime fim { get; set; }
        public int reason { get; set; }
        public string descricao { get; set; } = string.Empty;
        public bool pausaativa { get; set; }
        public int evt_number { get; set; }
        public bool prepausa { get; set; }
    }

    public class PausaLista : List<Pausa> { }
}
