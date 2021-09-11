using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APGLogs.Application.EventSourcedNormalizers;
using APGLogs.Application.ViewModels;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;
using FluentValidation.Results;

namespace APGLogs.Application.Interfaces
{
    public interface ICommunicationLogAppService : IDisposable
    {
        Task<IEnumerable<CommunicationLogViewModel>> GetAll();
        Task<CommunicationLogViewModel> GetById(Guid id);
        Task<PaginatedResult<CommunicationLogViewModel>> GetAllPaged(CommunicationLogFilter filter);
        Task<bool> CheckReplayAttach(CommunicationLogFilter filter);
        Task<ExportViewModel> GetAllExported(CommunicationLogFilter filter);
        Task Add(CommunicationLogViewModel communicationLogViewModel);
        Task Update(CommunicationLogViewModel communicationLogViewModel);
        Task Remove(Guid id);
    }
}
