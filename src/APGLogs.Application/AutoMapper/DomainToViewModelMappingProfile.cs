using AutoMapper;
using APGLogs.Application.ViewModels;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Models;

namespace APGLogs.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<ExceptionLog, ExceptionLogViewModel>().ReverseMap();
            CreateMap<PaginatedResult<ExceptionLog>, PaginatedResult<ExceptionLogViewModel>>().ReverseMap();

            CreateMap<ExceptionLogType, ExceptionLogTypeViewModel>().ReverseMap();
            CreateMap<PaginatedResult<ExceptionLogType>, PaginatedResult<ExceptionLogTypeViewModel>>().ReverseMap();

            CreateMap<CommunicationLog, CommunicationLogViewModel>().ReverseMap();
            CreateMap<PaginatedResult<CommunicationLog>, PaginatedResult<CommunicationLogViewModel>>().ReverseMap();

            CreateMap<CommunicationLogType, CommunicationLogTypeViewModel>().ReverseMap();
            CreateMap<PaginatedResult<CommunicationLogType>, PaginatedResult<CommunicationLogTypeViewModel>>().ReverseMap();

            CreateMap<SMSLog, SMSLogViewModel>().ReverseMap();
            CreateMap<PaginatedResult<SMSLog>, PaginatedResult<SMSLogViewModel>>().ReverseMap();

            CreateMap<EmailLog, EmailLogViewModel>().ReverseMap();
            CreateMap<PaginatedResult<EmailLog>, PaginatedResult<EmailLogViewModel>>().ReverseMap();

            CreateMap<PortalSessionAudit, PortalSessionAuditViewModel>().ReverseMap();
            CreateMap<PaginatedResult<PortalSessionAudit>, PaginatedResult<PortalSessionAuditViewModel>>().ReverseMap();

            CreateMap<PortalSessionAuditAction, PortalSessionAuditActionViewModel>().ReverseMap();
            CreateMap<PaginatedResult<PortalSessionAuditAction>, PaginatedResult<PortalSessionAuditActionViewModel>>().ReverseMap();

            CreateMap<AMSTransactionAudit, AMSTransactionAuditViewModel>().ReverseMap();
            CreateMap<PaginatedResult<AMSTransactionAudit>, PaginatedResult<AMSTransactionAuditViewModel>>().ReverseMap();

            CreateMap<AMSBalanceAudit, AMSBalanceAuditViewModel>().ReverseMap();
            CreateMap<PaginatedResult<AMSBalanceAudit>, PaginatedResult<AMSBalanceAuditViewModel>>().ReverseMap();

            CreateMap<ShadowBalanceAudit, ShadowBalanceAuditViewModel>().ReverseMap();
            CreateMap<PaginatedResult<ShadowBalanceAudit>, PaginatedResult<ShadowBalanceAuditViewModel>>().ReverseMap();





            CreateMap<ExportedCSVFile, ExportViewModel>().ReverseMap();
        }
    }
}
