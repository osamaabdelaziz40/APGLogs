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
    public class EmailLogAppService : IEmailLogAppService
    {
        private readonly IMapper _mapper;
        private readonly IEmailLogRepository _EmailLogRepository;
        private readonly ICSVManager _csvManager;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler _mediator;

        public EmailLogAppService(IMapper mapper,
                                  IEmailLogRepository EmailLogRepository,
                                  ICSVManager csvManager,
                                  IMediatorHandler mediator,
                                  IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _EmailLogRepository = EmailLogRepository;
            _csvManager = csvManager;
            _mediator = mediator;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<IEnumerable<EmailLogViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<EmailLogViewModel>>(await _EmailLogRepository.GetAll());
        }

        public async Task<PaginatedResult<EmailLogViewModel>> GetAllPaged(EmailLogFilter filter)
   => _mapper.Map<PaginatedResult<EmailLogViewModel>>(await _EmailLogRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false));

        public async Task<ExportViewModel> GetAllExported(EmailLogFilter filter)
        {
            filter.IsExport = true;
            var pagedResult = await _EmailLogRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false);

            var exportedCSV = await _csvManager.Export(pagedResult.Records, "ExportedEmailLogs");
            return _mapper.Map<ExportViewModel>(exportedCSV);
        }

        public async Task<EmailLogViewModel> GetById(Guid id)
        {
            return _mapper.Map<EmailLogViewModel>(await _EmailLogRepository.GetById(id));
        }

        public async Task Add(EmailLogViewModel EmailLogViewModel)
        {
            var exceptionLog = _mapper.Map<EmailLog>(EmailLogViewModel);
            exceptionLog.Id = Guid.NewGuid().ToString();
            await _EmailLogRepository.Add(exceptionLog);
        }

        public async Task Update(EmailLogViewModel EmailLogViewModel)
        {
            var EmailLog = _mapper.Map<EmailLog>(EmailLogViewModel);

            await _EmailLogRepository.Update(EmailLog);
        }

        public async Task Remove(Guid id)
        {
            await _EmailLogRepository.Remove(id);
        }
        public async Task RemoveRange(DateTime date)
        {
            await _EmailLogRepository.RemoveRange(date);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
