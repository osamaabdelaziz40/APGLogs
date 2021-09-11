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
    public interface IExceptionLogAppService : IDisposable
    {
        Task<IEnumerable<ExceptionLogViewModel>> GetAll();
        Task<PaginatedResult<ExceptionLogViewModel>> GetAllPaged(ExceptionLogFilter filter);
        Task<ExportViewModel> GetAllExported(ExceptionLogFilter filter);
        Task<ExceptionLogViewModel> GetById(Guid id);
        Task<IEnumerable<ExceptionLogViewModel>> GetByCommunicationLogId(Guid communicationLogId);
        Task Add(ExceptionLogViewModel exceptionLogViewModel);
        Task Update(ExceptionLogViewModel exceptionLogViewModel);
        Task Remove(Guid id);
    }
}
