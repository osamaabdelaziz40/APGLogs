using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;

namespace APGLogs.Domain.Interfaces
{
    public interface ICommunicationLogRepository
    {
        Task<CommunicationLog> GetById(Guid id);
        Task<IEnumerable<CommunicationLog>> GetAll();
        Task<PaginatedResult<CommunicationLog>> GetPaginatedResultAsync(CommunicationLogFilter filter);
        Task Add(CommunicationLog communicationLog);
        Task Update(CommunicationLog communicationLog);
        Task Remove(Guid id);
    }
}