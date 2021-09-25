using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;

namespace APGLogs.Domain.Interfaces
{
    public interface IAMSBalanceAuditRepository
    {
        Task<AMSBalanceAudit> GetById(Guid id);
        Task<IEnumerable<AMSBalanceAudit>> GetAll();
        Task<PaginatedResult<AMSBalanceAudit>> GetPaginatedResultAsync(AMSBalanceAuditFilter filter);
        Task Add(AMSBalanceAudit AMSBalanceAudit);
        Task Update(AMSBalanceAudit AMSBalanceAudit);
        Task Remove(Guid id);
        Task RemoveRange(DateTime date);
    }
}