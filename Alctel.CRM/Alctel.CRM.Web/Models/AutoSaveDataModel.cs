using System.Security.Cryptography.Xml;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Alctel.CRM.Web.Models;

public class AutoSaveDataModel
{
    public string? ConversationId { get; set; }
    public Int64 DemandTypeId { get; set; }
    public Int64 TicketCriticalityId { get; set; }
    public Int64 TicketStatusId { get; set; }
    public string? AnySolution { get; set; }
    public string? DemandObservation { get; set; }
    public string? ParentTicket { get; set; }
    public List<AttachmentData> AttachmentDatas { get; set; } = new List<AttachmentData>();
    public List<Attachment> Attachments { get; set; } = new List<Attachment>();
    public List<IFormFile>? Files { get; set; }
}

public class Attachment
{
    public string? FileStringByte { get; set; }    
    public string? FileType { get; set; }
    public string? FileName { get; set; }
}

public class AttachmentData
{
    public string? ContentDisposition { get; set; }
    public string? ContentType { get; set; }
    public string? FileName { get; set; }
    public string? FileContent { get; set; }
    public Stream? FileStream { get; set; }
    public byte[]? FileBytes { get; set; }
}
