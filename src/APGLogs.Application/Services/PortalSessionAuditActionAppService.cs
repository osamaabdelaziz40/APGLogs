using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using APGLogs.Application.EventSourcedNormalizers;
using APGLogs.Application.Interfaces;
using APGLogs.Application.ViewModels;
using APGLogs.Domain.Commands;
using APGLogs.Domain.Interfaces;
using APGLogs.Infra.Data.Repository.EventSourcing;
using FluentValidation.Results;
using NetDevPack.Mediator;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Models;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Interfaces;

namespace APGLogs.Application.Services
{
    public class PortalSessionAuditActionAppService : IPortalSessionAuditActionAppService
    {
        private readonly IMapper _mapper;
        private readonly IPortalSessionAuditActionRepository _PortalSessionAuditActionRepository;
        private readonly ICSVManager _csvManager;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler _mediator;

        public PortalSessionAuditActionAppService(IMapper mapper,
                                  IPortalSessionAuditActionRepository PortalSessionAuditActionRepository,
                                  ICSVManager csvManager,
                                  IMediatorHandler mediator,
                                  IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _PortalSessionAuditActionRepository = PortalSessionAuditActionRepository;
            _csvManager = csvManager;
            _mediator = mediator;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<IEnumerable<PortalSessionAuditActionViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<PortalSessionAuditActionViewModel>>(await _PortalSessionAuditActionRepository.GetAll());
        }

        public async Task<PortalSessionAuditActionViewModel> GetById(Guid id)
        {
            return _mapper.Map<PortalSessionAuditActionViewModel>(await _PortalSessionAuditActionRepository.GetById(id));
        }

        public async Task<PaginatedResult<PortalSessionAuditActionViewModel>> GetAllPaged(PortalSessionAuditActionFilter filter)
   => _mapper.Map<PaginatedResult<PortalSessionAuditActionViewModel>>(await _PortalSessionAuditActionRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false));

        public async Task<ExportViewModel> GetAllExported(PortalSessionAuditActionFilter filter)
        {
            filter.IsExport = true;
            var pagedResult = await _PortalSessionAuditActionRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false);

            var exportedCSV = await _csvManager.Export(pagedResult.Records, "ExportedPortalSessionAuditAction");
            return _mapper.Map<ExportViewModel>(exportedCSV);
        }
        public async Task Add(PortalSessionAuditActionViewModel PortalSessionAuditActionViewModel)
        {
            var exceptionLog = _mapper.Map<PortalSessionAuditAction>(PortalSessionAuditActionViewModel);
            exceptionLog.Id = Guid.NewGuid().ToString();
            await _PortalSessionAuditActionRepository.Add(exceptionLog);
        }

        public async Task Update(PortalSessionAuditActionViewModel PortalSessionAuditActionViewModel)
        {
            var PortalSessionAuditAction = _mapper.Map<PortalSessionAuditAction>(PortalSessionAuditActionViewModel);

            await _PortalSessionAuditActionRepository.Update(PortalSessionAuditAction);
        }

        public async Task Remove(Guid id)
        {
            await _PortalSessionAuditActionRepository.Remove(id);
        }
        public async Task RemoveRange(DateTime date)
        {
            await _PortalSessionAuditActionRepository.RemoveRange(date);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
