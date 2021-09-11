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
    public class CommunicationLogTypeAppService : ICommunicationLogTypeAppService
    {
        private readonly IMapper _mapper;
        private readonly ICommunicationLogTypeRepository _communicationLogTypeRepository;
        private readonly ICSVManager _csvManager;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler _mediator;

        public CommunicationLogTypeAppService(IMapper mapper,
                                  ICommunicationLogTypeRepository communicationLogTypeRepository,
                                  ICSVManager csvManager,
                                  IMediatorHandler mediator,
                                  IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _communicationLogTypeRepository = communicationLogTypeRepository;
            _csvManager = csvManager;
            _mediator = mediator;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<IEnumerable<CommunicationLogTypeViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<CommunicationLogTypeViewModel>>(await _communicationLogTypeRepository.GetAll());
        }

       

        public async Task<CommunicationLogTypeViewModel> GetById(Guid id)
        {
            return _mapper.Map<CommunicationLogTypeViewModel>(await _communicationLogTypeRepository.GetById(id));
        }

        public async Task Add(CommunicationLogTypeViewModel communicationLogTypeViewModel)
        {
            var exceptionLogType = _mapper.Map<CommunicationLogType>(communicationLogTypeViewModel);
            exceptionLogType.Id = Guid.NewGuid().ToString();
            await _communicationLogTypeRepository.Add(exceptionLogType);
        }

        public async Task Update(CommunicationLogTypeViewModel communicationLogTypeViewModel)
        {
            var communicationLogType = _mapper.Map<CommunicationLogType>(communicationLogTypeViewModel);

            await _communicationLogTypeRepository.Update(communicationLogType);
        }

        public async Task Remove(Guid id)
        {
            await _communicationLogTypeRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
