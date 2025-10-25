using System.ComponentModel;

namespace Alctel.CRM.Web.Models;

public class SlaAlertAgendaModel
{
    public Int64 UserId { get; set; }

    [DisplayName("Nome Feriado")]
    public string? HolidayName { get; set; }

    [DisplayName("Data do Feriado")]
    public DateTime HolidayDate { get; set; }
}
