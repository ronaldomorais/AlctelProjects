using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alctel.CRM.API.Entities;

public class TicketTransferCreateAPI
{
    public Int64 idChamado { get; set; }
    public Int64 idUsuarioOrigem { get; set; }
    public Int64 idFilaGT { get; set; }
    public Int64? idAreaDemanda { get; set; }
    public Int64? idLista { get; set; }
    public Int64? idListaItem { get; set; }
    public string? motivo { get; set; }
    public string? formulario { get; set; }
}
