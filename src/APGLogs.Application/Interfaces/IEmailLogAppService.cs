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
    public interface IEmailLogAppService : IDisposable
    {
        Task<IEnumerable<EmailLogViewModel>> GetAll();
        Task<EmailLogViewModel> GetById(Guid id);
        Task<PaginatedResult<EmailLogViewModel>> GetAllPaged(EmailLogFilter filter);
        Task<ExportViewModel> GetAllExported(EmailLogFilter filter);
        Task Add(EmailLogViewModel EmailLogViewModel);
        Task Update(EmailLogViewModel EmailLogViewModel);
        Task Remove(Guid id);
        Task RemoveRange(DateTime date);

    }
}
