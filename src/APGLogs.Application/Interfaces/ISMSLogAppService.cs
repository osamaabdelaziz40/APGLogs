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
    public interface ISMSLogAppService : IDisposable
    {
        Task<IEnumerable<SMSLogViewModel>> GetAll();
        Task<SMSLogViewModel> GetById(Guid id);
        Task<PaginatedResult<SMSLogViewModel>> GetAllPaged(SMSLogFilter filter);
        Task<ExportViewModel> GetAllExported(SMSLogFilter filter);
        Task Add(SMSLogViewModel SMSLogViewModel);
        Task Update(SMSLogViewModel SMSLogViewModel);
        Task Remove(Guid id);
        Task RemoveRange(DateTime date);
    }
}
