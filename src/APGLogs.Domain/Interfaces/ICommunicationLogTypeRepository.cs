using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;

namespace APGLogs.Domain.Interfaces
{
    public interface ICommunicationLogTypeRepository
    {
        Task<CommunicationLogType> GetById(Guid id);
        Task<IEnumerable<CommunicationLogType>> GetAll();
        Task Add(CommunicationLogType communicationLogType);
        Task Update(CommunicationLogType communicationLogType);
        Task Remove(Guid id);
    }
}