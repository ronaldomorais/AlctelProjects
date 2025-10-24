using Alctel.CRM.API.Entities;
using Alctel.CRM.Context.InMemory.Entities;
using Alctel.CRM.Context.InMemory.Entities.Classification;
using Alctel.CRM.Web.Models;
using Alctel.CRM.Web.Models.Classification;
using AutoMapper;

namespace Alctel.CRM.Web.Maps;

public class MapConfigProfile : Profile
{
    public MapConfigProfile()
    {
        CreateMap<Customer, CustomerModel>();
        CreateMap<CustomerModel, Customer>();

        CreateMap<User, UserModel>();
        CreateMap<UserModel, User>();

        CreateMap<AccessProfile, AccessProfileModel>();
        CreateMap<AccessProfileModel, AccessProfile>();

        CreateMap<ActionPermission, ActionPermissionModel>();
        CreateMap<ActionPermissionModel, ActionPermission>();

        CreateMap<Module, ModuleModel>();
        CreateMap<ModuleModel, Module>();

        CreateMap<ServiceUnit, ServiceUnitModel>();
        CreateMap<ServiceUnitModel, ServiceUnit>();

        CreateMap<Area, AreaModel>();
        CreateMap<AreaModel, Area>();

        CreateMap<ServiceLevel, ServiceLevelModel>();
        CreateMap<ServiceLevelModel, ServiceLevel>();

        CreateMap<Reason, ReasonModel>();
        CreateMap<ReasonModel, Reason>();

        CreateMap<ReasonList, ReasonListModel>();
        CreateMap<ReasonListModel, ReasonList>();

        //API
        CreateMap<UserAPI, UserModel>();
        CreateMap<UserModel, UserAPI>();

        CreateMap<ServiceUnitAPI, ServiceUnitModel>();
        CreateMap<ServiceUnitModel, ServiceUnitAPI>();

        CreateMap<AreaAPI, AreaModel>();
        CreateMap<AreaModel, AreaAPI>();

        CreateMap<ServiceLevelAPI, ServiceLevelModel>();
        CreateMap<ServiceLevelModel, ServiceLevelAPI>();

        CreateMap<DemandTypeAPI, DemandTypeModel>();
        CreateMap<DemandTypeModel, DemandTypeAPI>();

        CreateMap<AccessProfileAPI, AccessProfileModel>();
        CreateMap<AccessProfileModel, AccessProfileAPI>();

        CreateMap<LogDataReceived, LogDataReceivedModel>();
        CreateMap<LogDataReceivedModel, LogDataReceived>();

        CreateMap<CustomerAPI, CustomerModel>();
        CreateMap<CustomerModel, CustomerAPI>();

        CreateMap<CustomerCreateAPI, CustomerCreateModel>();
        CreateMap<CustomerCreateModel, CustomerCreateAPI>();

        CreateMap<ClassificationListAPI, ClassificationListModel>();
        CreateMap<ClassificationListModel, ClassificationListAPI>();

        CreateMap<ClassificationDemandAPI, ClassificationDemandModel>();
        CreateMap<ClassificationDemandModel, ClassificationDemandAPI>();

        CreateMap<ClassificationDemandTypeAPI, ClassificationDemandTypeModel>();
        CreateMap<ClassificationDemandTypeModel, ClassificationDemandTypeAPI>();

        CreateMap<ClassificationReasonAPI, ClassificationReasonModel>();
        CreateMap<ClassificationReasonModel, ClassificationReasonAPI>();

        CreateMap<ClassificationListItemsAPI, ClassificationListItemsModel>();
        CreateMap<ClassificationListItemsModel, ClassificationListItemsAPI>();

        CreateMap<ClassificationReasonChildrenAPI, ClassificationReasonChildrenModel>();
        CreateMap<ClassificationReasonChildrenModel, ClassificationReasonChildrenAPI>();

        CreateMap<TicketAPI, TicketModel>()
            .ForMember(dest => dest.AnySolution, o => o.MapFrom(s => s.AnySolution == "false" ? "0": "1"))
            //.ForMember(dest => dest.CustomerFullName, o => o.MapFrom(s => s.CustomerName))
            .ForMember(dest => dest.Customer, input => input.MapFrom(i => new Customer
            {
                Id = i.CustomerId != null ? Int64.Parse(i.CustomerId) : 0,
                FirstName = i.CustomerName,
                SocialAffectionateName= i.SocialAffectionateName,
                Cpf = i.Cpf,
            }));

        CreateMap<TicketModel, TicketAPI>();

        CreateMap<TicketStatusAPI, TicketStatusModel>();
        CreateMap<TicketStatusModel, TicketStatusAPI>();

        CreateMap<TicketCriticalityAPI, TicketCriticalityModel>();
        CreateMap<TicketCriticalityModel, TicketCriticalityAPI>();

        CreateMap<TicketModel, TicketCreateAPI>()
            .ForMember(dest => dest.CustomerId, o => o.MapFrom(s => s.Customer.Id))
            .ForMember(dest => dest.Cpf, o => o.MapFrom(s => s.Customer.Cpf))
            .ForMember(dest => dest.FirstName, o => o.MapFrom(s => s.Customer.FirstName))
            //.ForMember(dest => dest.TicketStatusId, o => o.MapFrom(s => s.TicketStatus != null ? Int64.Parse(s.TicketStatus) : 0))
            //.ForMember(dest => dest.TicketCriticalityId, o => o.MapFrom(s => s.TicketCriticality != null ? Int64.Parse(s.TicketCriticality) : 0))
            .ForMember(dest => dest.UserId, o => o.MapFrom(s => s.User != null ? Int64.Parse(s.User) : 0))
            .ForMember(dest => dest.AnySolution, o => o.MapFrom(s => s.AnySolution == null ? false : s.AnySolution == "0" ? false : true))
            //.ForMember(dest => dest.DemandTypeId, o => o.MapFrom(s => s.DemandType != null ? Int64.Parse(s.DemandType) : 0))
            .ForMember(dest => dest.ParentTicket, o => o.MapFrom(s => string.IsNullOrEmpty(s.ParentTicket) ? "" : s.ParentTicket))
            .ForMember(dest => dest.CustomerNavigation, o => o.MapFrom(s => string.IsNullOrEmpty(s.CustomerNavigation) ? "" : s.CustomerNavigation));

        CreateMap<TicketModel, TicketClassificationCreateAPI>()
            .ForMember(dest => dest.TicketId, o => o.MapFrom(s => s.Id))
            .ForMember(dest => dest.UserId, o => o.MapFrom(s => s.User != null ? Int64.Parse(s.User) : 0))
            .ForMember(dest => dest.Order, o => o.Ignore())
            .ForMember(dest => dest.ClassificationReasonId, o => o.MapFrom(s => s.ClassificationTree.ClassificationReasonId))
            .ForMember(dest => dest.ClassificationReasonListId, o => o.MapFrom(s => s.ClassificationTree.ClassificationReasonListItemId));

        CreateMap<TicketQueueGTAPI, TicketQueueGTModel>();
        CreateMap<TicketQueueGTModel, TicketQueueGTAPI>();
        CreateMap<TicketAttachmentAPI, TicketAttachmentModel>();
        CreateMap<TicketAttachmentModel, TicketAttachmentAPI>();
        CreateMap<AgentsAssistantsDataAPI, AgentsAssistantsDataModel>();

        CreateMap<TicketAssignmentCreateModel, TicketAssignmentUserCreateAPI>();
        CreateMap<TicketAssignmentCreateModel, TicketAssignmentQueueUserCreateAPI>();

        CreateMap<TicketAgentStatusAPI, TicketStatusModel>();
        CreateMap<TicketAssistentStatusAPI, TicketStatusModel>();

        CreateMap<TicketClassificationListModel, TicketClassificationListAPI>();
        CreateMap<TicketClassificationListAPI, TicketClassificationListModel>();

        CreateMap<TicketClassificationListItemsAPI, TicketClassificationListItemsModel>();
        CreateMap<TicketClassificationListItemsModel, TicketClassificationListItemsAPI>();

        CreateMap<TicketClassificationListItemModel, TicketClassificationListItemAPI>();
        CreateMap<TicketClassificationListItemAPI, TicketClassificationListItemModel>();

        CreateMap<TicketClassificationManifestationTypeAPI, TicketClassificationManifestationTypeModel>();

        CreateMap<TicketClassificationProgramAPI, TicketClassificationProgramModel>();

        CreateMap<TicketClassificationReasonListAPI, TicketClassificationReasonListModel>();

        CreateMap<TicketClassificationServiceAPI, TicketClassificationServiceModel>();

        CreateMap<TicketReasonCreateModel, TicketReasonCreateAPI>();
        CreateMap<TicketClassificationReasonCreateModel, TicketClassificationReasonCreateAPI>();

        CreateMap<TicketReasonModel, TicketReasonAPI>();
        CreateMap<TicketClassificationCreateModel, TicketClassificationAPI>();
    }
}
