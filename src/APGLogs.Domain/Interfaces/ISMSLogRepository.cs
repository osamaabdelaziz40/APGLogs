using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;

namespace APGLogs.Domain.Interfaces
{
    public interface ISMSLogRepository
    {
        Task<SMSLog> GetById(Guid id);
        Task<IEnumerable<SMSLog>> GetAll();
        Task<PaginatedResult<SMSLog>> GetPaginatedResultAsync(SMSLogFilter filter);
        Task Add(SMSLog SMSLog);
        Task Update(SMSLog SMSLog);
        Task Remove(Guid id);
    }
}