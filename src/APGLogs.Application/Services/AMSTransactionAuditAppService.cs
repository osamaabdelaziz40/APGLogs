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
    public class AMSTransactionAuditAppService : IAMSTransactionAuditAppService
    {
        private readonly IMapper _mapper;
        private readonly IAMSTransactionAuditRepository _AMSTransactionAuditRepository;
        private readonly ICSVManager _csvManager;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler _mediator;

        public AMSTransactionAuditAppService(IMapper mapper,
                                  IAMSTransactionAuditRepository AMSTransactionAuditRepository,
                                  ICSVManager csvManager,
                                  IMediatorHandler mediator,
                                  IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _AMSTransactionAuditRepository = AMSTransactionAuditRepository;
            _csvManager = csvManager;
            _mediator = mediator;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<IEnumerable<AMSTransactionAuditViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<AMSTransactionAuditViewModel>>(await _AMSTransactionAuditRepository.GetAll());
        }

        public async Task<AMSTransactionAuditViewModel> GetById(Guid id)
        {
            return _mapper.Map<AMSTransactionAuditViewModel>(await _AMSTransactionAuditRepository.GetById(id));
        }

        public async Task<PaginatedResult<AMSTransactionAuditViewModel>> GetAllPaged(AMSTransactionAuditFilter filter)
   => _mapper.Map<PaginatedResult<AMSTransactionAuditViewModel>>(await _AMSTransactionAuditRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false));

        public async Task<ExportViewModel> GetAllExported(AMSTransactionAuditFilter filter)
        {
            filter.IsExport = true;
            var pagedResult = await _AMSTransactionAuditRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false);

            var exportedCSV = await _csvManager.Export(pagedResult.Records, "ExportedAMSTransactionAudit");
            return _mapper.Map<ExportViewModel>(exportedCSV);
        }
        public async Task Add(AMSTransactionAuditViewModel AMSTransactionAuditViewModel)
        {
            var exceptionLog = _mapper.Map<AMSTransactionAudit>(AMSTransactionAuditViewModel);
            exceptionLog.Id = Guid.NewGuid().ToString();
            await _AMSTransactionAuditRepository.Add(exceptionLog);
        }

        public async Task Update(AMSTransactionAuditViewModel AMSTransactionAuditViewModel)
        {
            var AMSTransactionAudit = _mapper.Map<AMSTransactionAudit>(AMSTransactionAuditViewModel);

            await _AMSTransactionAuditRepository.Update(AMSTransactionAudit);
        }

        public async Task Remove(Guid id)
        {
            await _AMSTransactionAuditRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
