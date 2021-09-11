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
    public interface IPortalSessionAuditAppService : IDisposable
    {
        Task<IEnumerable<PortalSessionAuditViewModel>> GetAll();
        Task<PortalSessionAuditViewModel> GetById(Guid id);
        Task Add(PortalSessionAuditViewModel PortalSessionAuditViewModel);
        Task Update(PortalSessionAuditViewModel PortalSessionAuditViewModel);
        Task Remove(Guid id);
    }
}
