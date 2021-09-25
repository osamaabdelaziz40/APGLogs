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
    public class AMSBalanceAuditAppService : IAMSBalanceAuditAppService
    {
        private readonly IMapper _mapper;
        private readonly IAMSBalanceAuditRepository _AMSBalanceAuditRepository;
        private readonly ICSVManager _csvManager;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler _mediator;

        public AMSBalanceAuditAppService(IMapper mapper,
                                  IAMSBalanceAuditRepository AMSBalanceAuditRepository,
                                  ICSVManager csvManager,
                                  IMediatorHandler mediator,
                                  IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _AMSBalanceAuditRepository = AMSBalanceAuditRepository;
            _csvManager = csvManager;
            _mediator = mediator;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<IEnumerable<AMSBalanceAuditViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<AMSBalanceAuditViewModel>>(await _AMSBalanceAuditRepository.GetAll());
        }

        public async Task<AMSBalanceAuditViewModel> GetById(Guid id)
        {
            return _mapper.Map<AMSBalanceAuditViewModel>(await _AMSBalanceAuditRepository.GetById(id));
        }

        public async Task<PaginatedResult<AMSBalanceAuditViewModel>> GetAllPaged(AMSBalanceAuditFilter filter)
   => _mapper.Map<PaginatedResult<AMSBalanceAuditViewModel>>(await _AMSBalanceAuditRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false));

        public async Task<ExportViewModel> GetAllExported(AMSBalanceAuditFilter filter)
        {
            filter.IsExport = true;
            var pagedResult = await _AMSBalanceAuditRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false);

            var exportedCSV = await _csvManager.Export(pagedResult.Records, "ExportedAMSBalanceAudit");
            return _mapper.Map<ExportViewModel>(exportedCSV);
        }
        public async Task Add(AMSBalanceAuditViewModel AMSBalanceAuditViewModel)
        {
            var exceptionLog = _mapper.Map<AMSBalanceAudit>(AMSBalanceAuditViewModel);
            exceptionLog.Id = Guid.NewGuid().ToString();
            await _AMSBalanceAuditRepository.Add(exceptionLog);
        }

        public async Task Update(AMSBalanceAuditViewModel AMSBalanceAuditViewModel)
        {
            var AMSBalanceAudit = _mapper.Map<AMSBalanceAudit>(AMSBalanceAuditViewModel);

            await _AMSBalanceAuditRepository.Update(AMSBalanceAudit);
        }

        public async Task Remove(Guid id)
        {
            await _AMSBalanceAuditRepository.Remove(id);
        }
        public async Task RemoveRange(DateTime date)
        {
            await _AMSBalanceAuditRepository.RemoveRange(date);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
