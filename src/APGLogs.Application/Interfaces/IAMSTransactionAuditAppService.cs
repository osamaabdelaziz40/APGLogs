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
    public interface IAMSTransactionAuditAppService : IDisposable
    {
        Task<IEnumerable<AMSTransactionAuditViewModel>> GetAll();
        Task<AMSTransactionAuditViewModel> GetById(Guid id);
        Task<PaginatedResult<AMSTransactionAuditViewModel>> GetAllPaged(AMSTransactionAuditFilter filter);
        Task<ExportViewModel> GetAllExported(AMSTransactionAuditFilter filter);
        Task Add(AMSTransactionAuditViewModel AMSTransactionAuditViewModel);
        Task Update(AMSTransactionAuditViewModel AMSTransactionAuditViewModel);
        Task Remove(Guid id);
        Task RemoveRange(DateTime date);

    }
}
