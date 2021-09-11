using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;

namespace APGLogs.Domain.Interfaces
{
    public interface IAMSTransactionAuditRepository
    {
        Task<AMSTransactionAudit> GetById(Guid id);
        Task<IEnumerable<AMSTransactionAudit>> GetAll();
        Task<PaginatedResult<AMSTransactionAudit>> GetPaginatedResultAsync(AMSTransactionAuditFilter filter);
        Task Add(AMSTransactionAudit AMSTransactionAudit);
        Task Update(AMSTransactionAudit AMSTransactionAudit);
        Task Remove(Guid id);
    }
}