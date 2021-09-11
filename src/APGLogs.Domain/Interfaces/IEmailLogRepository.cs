using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;

namespace APGLogs.Domain.Interfaces
{
    public interface IEmailLogRepository
    {
        Task<EmailLog> GetById(Guid id);
        Task<IEnumerable<EmailLog>> GetAll();
        Task<PaginatedResult<EmailLog>> GetPaginatedResultAsync(EmailLogFilter filter);
        Task Add(EmailLog EmailLog);
        Task Update(EmailLog EmailLog);
        Task Remove(Guid id);
    }
}