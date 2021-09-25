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
using APGLogs.Constant;

namespace APGLogs.Application.Services
{
    public class CommunicationLogAppService : ICommunicationLogAppService
    {
        private readonly IMapper _mapper;
        private readonly ICommunicationLogRepository _communicationLogRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;
        private readonly ICSVManager _csvManager;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler _mediator;

        public CommunicationLogAppService(IMapper mapper,
                                  ICommunicationLogRepository communicationLogRepository,
                                  ICSVManager csvManager,
                                  IMediatorHandler mediator,
                                  IExceptionLogRepository exceptionLogRepository,
                                  IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _communicationLogRepository = communicationLogRepository;
            _csvManager = csvManager;
            _mediator = mediator;
            _eventStoreRepository = eventStoreRepository;
            _exceptionLogRepository = exceptionLogRepository;
        }

        public async Task<IEnumerable<CommunicationLogViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<CommunicationLogViewModel>>(await _communicationLogRepository.GetAll());
        }

        public async Task<PaginatedResult<CommunicationLogViewModel>> GetAllPaged(CommunicationLogFilter filter)
   => _mapper.Map<PaginatedResult<CommunicationLogViewModel>>(await _communicationLogRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false));

        public async Task<ApiResponseVM> CheckReplayAttach(CommunicationLogFilter filter)
        {
            //return 

            var res = await _communicationLogRepository.CheckReplayAttach(filter);//.ConfigureAwait(false);
            if (res.Code == 300)
            {
                var exceptionLog = new ExceptionLog();
                exceptionLog.Id = Guid.NewGuid().ToString();
                exceptionLog.Message = ExceptionContant.AttackError_Message+ filter.DateTime.ToString();
                //exceptionLog.Source = exception.Source;
                //exceptionLog.StackTrace = exception.StackTrace;
                exceptionLog.InnerException = ExceptionContant.AttackError_InnerException+res.Data.ToString();
                exceptionLog.DateTime = DateTime.Now;
                exceptionLog.CommunicationLogId = res.Data.ToString();
                exceptionLog.ExceptionType = CommonCostant.APGException;
                await _exceptionLogRepository.Add(exceptionLog);
                return res;
            }
            return res;
        }



        public async Task<ExportViewModel> GetAllExported(CommunicationLogFilter filter)
        {
            filter.IsExport = true;
            var pagedResult = await _communicationLogRepository.GetPaginatedResultAsync(filter).ConfigureAwait(false);

            var exportedCSV = await _csvManager.Export(pagedResult.Records, "ExportedCommunicationLogs");
            return _mapper.Map<ExportViewModel>(exportedCSV);
        }

        public async Task<CommunicationLogViewModel> GetById(Guid id)
        {
            return _mapper.Map<CommunicationLogViewModel>(await _communicationLogRepository.GetById(id));
        }

        public async Task Add(CommunicationLogViewModel communicationLogViewModel)
        {
            var exceptionLog = _mapper.Map<CommunicationLog>(communicationLogViewModel);
            exceptionLog.Id = Guid.NewGuid().ToString();
            await _communicationLogRepository.Add(exceptionLog);
        }

        public async Task Update(CommunicationLogViewModel communicationLogViewModel)
        {
            var communicationLog = _mapper.Map<CommunicationLog>(communicationLogViewModel);

            await _communicationLogRepository.Update(communicationLog);
        }

        public async Task Remove(Guid id)
        {
            await _communicationLogRepository.Remove(id);
        }
        public async Task RemoveRange(DateTime date)
        {
            await _communicationLogRepository.RemoveRange(date);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
