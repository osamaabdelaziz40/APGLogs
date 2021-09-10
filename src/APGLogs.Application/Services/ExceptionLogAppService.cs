using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using APGLogs.Application.Interfaces;
using APGLogs.Application.ViewModels;
using APGLogs.Domain.Interfaces;
using APGLogs.Infra.Data.Repository.EventSourcing;
using NetDevPack.Mediator;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Models;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Interfaces;

namespace APGLogs.Application.Services
{
    public class ExceptionLogAppService : IExceptionLogAppService
    {
        private readonly IMapper _mapper;
        private readonly IExceptionLogRepository _exceptionLogRepository;
        private readonly ICSVManager _csvManager;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler _mediator;

        public ExceptionLogAppService(IMapper mapper,
                                  IExceptionLogRepository exceptionLogRepository,
                                  ICSVManager csvManager,
                                  IMediatorHandler mediator,
                                  IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _exceptionLogRepository = exceptionLogRepository;
            _csvManager = csvManager;
            _mediator = mediator;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<IEnumerable<ExceptionLogViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<ExceptionLogViewModel>>(await _exceptionLogRepository.GetAll());
        }

        public async Task<ExceptionLogViewModel> GetById(Guid id)
        {
            return _mapper.Map<ExceptionLogViewModel>(await _exceptionLogRepository.GetById(id));
        }

        public async Task<IEnumerable<ExceptionLogViewModel>> GetByCommunicationLogId(Guid id)
        {
            return _mapper.Map<IEnumerable<ExceptionLogViewModel>>(await _exceptionLogRepository.GetByIdCommunicationLogId(id));
        }

        public async Task<PaginatedResult<ExceptionLogViewModel>> GetAllPaged(ExceptionLogFilter filter)
   => _mapper.Map<PaginatedResult<ExceptionLogViewModel>>(await _exceptionLogRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false));

        public async Task<ExportViewModel> GetAllExported(ExceptionLogFilter filter)
        {
            filter.IsExport = true;
            var pagedResult = await _exceptionLogRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false);

            var exportedCSV = await _csvManager.Export(pagedResult.Records, "ExportedExceptionLogs");
            return _mapper.Map<ExportViewModel>(exportedCSV);
        }

        public async Task Add(ExceptionLogViewModel exceptionLogViewModel)
        {
            var exceptionLog = _mapper.Map<ExceptionLog>(exceptionLogViewModel);
            exceptionLog.Id = Guid.NewGuid().ToString();
            await _exceptionLogRepository.Add(exceptionLog);
        }

        public async Task Update(ExceptionLogViewModel exceptionLogViewModel)
        {
            var exceptionLog = _mapper.Map<ExceptionLog>(exceptionLogViewModel);

            await _exceptionLogRepository.Update(exceptionLog);
        }

        public async Task Remove(Guid id)
        {
            await _exceptionLogRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
