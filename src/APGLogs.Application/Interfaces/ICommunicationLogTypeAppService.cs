using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APGLogs.Application.EventSourcedNormalizers;
using APGLogs.Application.ViewModels;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;
using FluentValidation.Results;

namespace APGLogs.Application.Interfaces
{
    public interface ICommunicationLogTypeAppService : IDisposable
    {
        Task<IEnumerable<CommunicationLogTypeViewModel>> GetAll();
        Task<CommunicationLogTypeViewModel> GetById(Guid id);
        Task Add(CommunicationLogTypeViewModel communicationLogTypeViewModel);
        Task Update(CommunicationLogTypeViewModel communicationLogTypeViewModel);
        Task Remove(Guid id);
    }
}
