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
    public interface IPortalSessionAuditActionAppService : IDisposable
    {
        Task<IEnumerable<PortalSessionAuditActionViewModel>> GetAll();
        Task<PortalSessionAuditActionViewModel> GetById(Guid id);
        Task<PaginatedResult<PortalSessionAuditActionViewModel>> GetAllPaged(PortalSessionAuditActionFilter filter);
        Task<ExportViewModel> GetAllExported(PortalSessionAuditActionFilter filter);
        Task Add(PortalSessionAuditActionViewModel PortalSessionAuditActionViewModel);
        Task Update(PortalSessionAuditActionViewModel PortalSessionAuditActionViewModel);
        Task Remove(Guid id);
        Task RemoveRange(DateTime date);
    }
}
