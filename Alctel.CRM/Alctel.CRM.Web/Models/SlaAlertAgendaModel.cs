using System.ComponentModel;

namespace Alctel.CRM.Web.Models;

public class SlaAlertAgendaModel
{
    [DisplayName("Nome Feriado")]
    public string? HolidayName { get; set; }

    [DisplayName("Data do Feriado")]
    public DateTime HolidayDate { get; set; }
}
