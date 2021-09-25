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
    public class ShadowBalanceAuditAppService : IShadowBalanceAuditAppService
    {
        private readonly IMapper _mapper;
        private readonly IShadowBalanceAuditRepository _ShadowBalanceAuditRepository;
        private readonly ICSVManager _csvManager;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler _mediator;

        public ShadowBalanceAuditAppService(IMapper mapper,
                                  IShadowBalanceAuditRepository ShadowBalanceAuditRepository,
                                  ICSVManager csvManager,
                                  IMediatorHandler mediator,
                                  IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _ShadowBalanceAuditRepository = ShadowBalanceAuditRepository;
            _csvManager = csvManager;
            _mediator = mediator;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<IEnumerable<ShadowBalanceAuditViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<ShadowBalanceAuditViewModel>>(await _ShadowBalanceAuditRepository.GetAll());
        }

        public async Task<ShadowBalanceAuditViewModel> GetById(Guid id)
        {
            return _mapper.Map<ShadowBalanceAuditViewModel>(await _ShadowBalanceAuditRepository.GetById(id));
        }

        public async Task<PaginatedResult<ShadowBalanceAuditViewModel>> GetAllPaged(ShadowBalanceAuditFilter filter)
   => _mapper.Map<PaginatedResult<ShadowBalanceAuditViewModel>>(await _ShadowBalanceAuditRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false));

        public async Task<ExportViewModel> GetAllExported(ShadowBalanceAuditFilter filter)
        {
            filter.IsExport = true;
            var pagedResult = await _ShadowBalanceAuditRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false);

            var exportedCSV = await _csvManager.Export(pagedResult.Records, "ExportedShadowBalanceAudit");
            return _mapper.Map<ExportViewModel>(exportedCSV);
        }
        public async Task Add(ShadowBalanceAuditViewModel ShadowBalanceAuditViewModel)
        {
            var exceptionLog = _mapper.Map<ShadowBalanceAudit>(ShadowBalanceAuditViewModel);
            exceptionLog.Id = Guid.NewGuid().ToString();
            await _ShadowBalanceAuditRepository.Add(exceptionLog);
        }

        public async Task Update(ShadowBalanceAuditViewModel ShadowBalanceAuditViewModel)
        {
            var ShadowBalanceAudit = _mapper.Map<ShadowBalanceAudit>(ShadowBalanceAuditViewModel);

            await _ShadowBalanceAuditRepository.Update(ShadowBalanceAudit);
        }

        public async Task Remove(Guid id)
        {
            await _ShadowBalanceAuditRepository.Remove(id);
        }
        public async Task RemoveRange(DateTime date)
        {
            await _ShadowBalanceAuditRepository.RemoveRange(date);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
