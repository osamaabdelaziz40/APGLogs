using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;

namespace APGLogs.Domain.Interfaces
{
    public interface IPortalSessionAuditRepository
    {
        Task<PortalSessionAudit> GetById(Guid id);
        Task<IEnumerable<PortalSessionAudit>> GetAll();
        Task Add(PortalSessionAudit PortalSessionAudit);
        Task Update(PortalSessionAudit PortalSessionAudit);
        Task Remove(Guid id);
    }
}