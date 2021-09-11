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
    public interface IShadowBalanceAuditAppService : IDisposable
    {
        Task<IEnumerable<ShadowBalanceAuditViewModel>> GetAll();
        Task<ShadowBalanceAuditViewModel> GetById(Guid id);
        Task<PaginatedResult<ShadowBalanceAuditViewModel>> GetAllPaged(ShadowBalanceAuditFilter filter);
        Task<ExportViewModel> GetAllExported(ShadowBalanceAuditFilter filter);
        Task Add(ShadowBalanceAuditViewModel ShadowBalanceAuditViewModel);
        Task Update(ShadowBalanceAuditViewModel ShadowBalanceAuditViewModel);
        Task Remove(Guid id);
    }
}
