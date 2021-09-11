using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;

namespace APGLogs.Domain.Interfaces
{
    public interface IPortalSessionAuditActionRepository
    {
        Task<PortalSessionAuditAction> GetById(Guid id);
        Task<IEnumerable<PortalSessionAuditAction>> GetAll();
        Task<PaginatedResult<PortalSessionAuditAction>> GetPaginatedResultAsync(PortalSessionAuditActionFilter filter);
        Task Add(PortalSessionAuditAction PortalSessionAuditAction);
        Task Update(PortalSessionAuditAction PortalSessionAuditAction);
        Task Remove(Guid id);
    }
}