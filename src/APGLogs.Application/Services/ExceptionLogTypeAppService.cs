using APGLogs.Application.Interfaces;
using APGLogs.Application.ViewModels;
using APGLogs.Domain.Interfaces;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Interfaces;
using APGLogs.Infra.Data.Repository.EventSourcing;
using AutoMapper;
using NetDevPack.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGLogs.Application.Services
{
    public class ExceptionLogTypeAppService : IExceptionLogTypeAppService
    {
        private readonly IMapper _mapper;
        private readonly IExceptionLogTypeRepository _exceptionLogTypeRepository;
        private readonly ICSVManager _csvManager;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler _mediator;

        public ExceptionLogTypeAppService(IMapper mapper,
                                  IExceptionLogTypeRepository exceptionLogTypeRepository,
                                  ICSVManager csvManager,
                                  IMediatorHandler mediator,
                                  IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _exceptionLogTypeRepository = exceptionLogTypeRepository;
            _csvManager = csvManager;
            _mediator = mediator;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<IEnumerable<ExceptionLogTypeViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<ExceptionLogTypeViewModel>>(await _exceptionLogTypeRepository.GetAll());
        }

        public async Task<ExceptionLogTypeViewModel> GetById(Guid id)
        {
            return _mapper.Map<ExceptionLogTypeViewModel>(await _exceptionLogTypeRepository.GetById(id));
        }

        public async Task Add(ExceptionLogTypeViewModel exceptionLogTypeViewModel)
        {
            var exceptionLogType = _mapper.Map<ExceptionLogType>(exceptionLogTypeViewModel);
            exceptionLogType.Id = Guid.NewGuid().ToString();
            await _exceptionLogTypeRepository.Add(exceptionLogType);
        }

        public async Task Update(ExceptionLogTypeViewModel exceptionLogTypeViewModel)
        {
            var exceptionLogType = _mapper.Map<ExceptionLogType>(exceptionLogTypeViewModel);

            await _exceptionLogTypeRepository.Update(exceptionLogType);
        }

        public async Task Remove(Guid id)
        {
            await _exceptionLogTypeRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
