using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;
using NetDevPack.Data;

namespace APGLogs.Domain.Interfaces
{
    public interface IExceptionLogTypeRepository
    {
        Task<ExceptionLogType> GetById(Guid id);
        Task<IEnumerable<ExceptionLogType>> GetAll();
        Task Add(ExceptionLogType exceptionLogType);
        Task Update(ExceptionLogType exceptionLogType);
        Task Remove(Guid id);
    }
}