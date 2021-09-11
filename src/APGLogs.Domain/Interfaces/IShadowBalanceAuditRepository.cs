using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;

namespace APGLogs.Domain.Interfaces
{
    public interface IShadowBalanceAuditRepository
    {
        Task<ShadowBalanceAudit> GetById(Guid id);
        Task<IEnumerable<ShadowBalanceAudit>> GetAll();
        Task<PaginatedResult<ShadowBalanceAudit>> GetPaginatedResultAsync(ShadowBalanceAuditFilter filter);
        Task Add(ShadowBalanceAudit ShadowBalanceAudit);
        Task Update(ShadowBalanceAudit ShadowBalanceAudit);
        Task Remove(Guid id);
    }
}