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
    public class SMSLogAppService : ISMSLogAppService
    {
        private readonly IMapper _mapper;
        private readonly ISMSLogRepository _SMSLogRepository;
        private readonly ICSVManager _csvManager;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler _mediator;

        public SMSLogAppService(IMapper mapper,
                                  ISMSLogRepository SMSLogRepository,
                                  ICSVManager csvManager,
                                  IMediatorHandler mediator,
                                  IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _SMSLogRepository = SMSLogRepository;
            _csvManager = csvManager;
            _mediator = mediator;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<IEnumerable<SMSLogViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<SMSLogViewModel>>(await _SMSLogRepository.GetAll());
        }

        public async Task<PaginatedResult<SMSLogViewModel>> GetAllPaged(SMSLogFilter filter)
   => _mapper.Map<PaginatedResult<SMSLogViewModel>>(await _SMSLogRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false));

        public async Task<ExportViewModel> GetAllExported(SMSLogFilter filter)
        {
            filter.IsExport = true;
            var pagedResult = await _SMSLogRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false);

            var exportedCSV = await _csvManager.Export(pagedResult.Records, "ExportedSMSLogs");
            return _mapper.Map<ExportViewModel>(exportedCSV);
        }

        public async Task<SMSLogViewModel> GetById(Guid id)
        {
            return _mapper.Map<SMSLogViewModel>(await _SMSLogRepository.GetById(id));
        }

        public async Task Add(SMSLogViewModel SMSLogViewModel)
        {
            var exceptionLog = _mapper.Map<SMSLog>(SMSLogViewModel);
            exceptionLog.Id = Guid.NewGuid().ToString();
            await _SMSLogRepository.Add(exceptionLog);
        }

        public async Task Update(SMSLogViewModel SMSLogViewModel)
        {
            var SMSLog = _mapper.Map<SMSLog>(SMSLogViewModel);

            await _SMSLogRepository.Update(SMSLog);
        }

        public async Task Remove(Guid id)
        {
            await _SMSLogRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
