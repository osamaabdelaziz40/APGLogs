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
            CreateMap<CommunicationLog, CommunicationLogViewModel>().ReverseMap();
            CreateMap<PaginatedResult<CommunicationLog>, PaginatedResult<CommunicationLogViewModel>>().ReverseMap();

            CreateMap<ExportedCSVFile, ExportViewModel>().ReverseMap();
        }
    }
}
