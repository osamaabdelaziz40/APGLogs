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
    public interface IAMSBalanceAuditAppService : IDisposable
    {
        Task<IEnumerable<AMSBalanceAuditViewModel>> GetAll();
        Task<AMSBalanceAuditViewModel> GetById(Guid id);
        Task<PaginatedResult<AMSBalanceAuditViewModel>> GetAllPaged(AMSBalanceAuditFilter filter);
        Task<ExportViewModel> GetAllExported(AMSBalanceAuditFilter filter);
        Task Add(AMSBalanceAuditViewModel AMSBalanceAuditViewModel);
        Task Update(AMSBalanceAuditViewModel AMSBalanceAuditViewModel);
        Task Remove(Guid id);
        Task RemoveRange(DateTime date);
    }
}
