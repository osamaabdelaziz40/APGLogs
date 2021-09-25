using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;
using NetDevPack.Data;

namespace APGLogs.Domain.Interfaces
{
    public interface IExceptionLogRepository
    {
        Task<ExceptionLog> GetById(Guid id);
        Task<IEnumerable<ExceptionLog>> GetByIdCommunicationLogId(Guid communicationLogId);
        Task<IEnumerable<ExceptionLog>> GetAll();
        Task<PaginatedResult<ExceptionLog>> GetPaginatedResultAsync(ExceptionLogFilter filter);
        Task Add(ExceptionLog exceptionLog);
        Task Update(ExceptionLog exceptionLog);
        Task Remove(Guid id);
        Task RemoveRange(DateTime date);
    }
}