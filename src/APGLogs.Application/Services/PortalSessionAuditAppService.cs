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
    public class PortalSessionAuditAppService : IPortalSessionAuditAppService
    {
        private readonly IMapper _mapper;
        private readonly IPortalSessionAuditRepository _PortalSessionAuditRepository;
        private readonly ICSVManager _csvManager;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler _mediator;

        public PortalSessionAuditAppService(IMapper mapper,
                                  IPortalSessionAuditRepository PortalSessionAuditRepository,
                                  ICSVManager csvManager,
                                  IMediatorHandler mediator,
                                  IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _PortalSessionAuditRepository = PortalSessionAuditRepository;
            _csvManager = csvManager;
            _mediator = mediator;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<IEnumerable<PortalSessionAuditViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<PortalSessionAuditViewModel>>(await _PortalSessionAuditRepository.GetAll());
        }

        public async Task<PortalSessionAuditViewModel> GetById(Guid id)
        {
            return _mapper.Map<PortalSessionAuditViewModel>(await _PortalSessionAuditRepository.GetById(id));
        }

        public async Task Add(PortalSessionAuditViewModel PortalSessionAuditViewModel)
        {
            var exceptionLog = _mapper.Map<PortalSessionAudit>(PortalSessionAuditViewModel);
            exceptionLog.Id = Guid.NewGuid().ToString();
            await _PortalSessionAuditRepository.Add(exceptionLog);
        }

        public async Task Update(PortalSessionAuditViewModel PortalSessionAuditViewModel)
        {
            var PortalSessionAudit = _mapper.Map<PortalSessionAudit>(PortalSessionAuditViewModel);

            await _PortalSessionAuditRepository.Update(PortalSessionAudit);
        }

        public async Task Remove(Guid id)
        {
            await _PortalSessionAuditRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
