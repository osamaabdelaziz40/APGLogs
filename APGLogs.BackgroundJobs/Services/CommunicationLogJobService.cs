using APGLogs.Application.Interfaces;
using APGLogs.BackgroundJobs.Interfaces;
using APGLogs.Domain.Interfaces;
using APGLogs.DomainHelper.Filter;
using APGLogs.Infra.Data.Repository.EventSourcing;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGLogs.BackgroundJobs.Services
{
    public class CommunicationLogJobService : ICommunicationLogJobService
    {
        private readonly IMapper _mapper;
        private readonly ICommunicationLogAppService _CommunicationLogAppService;
        private readonly IBackgroundClearTaskSettings _BackgroundClearTaskSettings;
        public CommunicationLogJobService(IMapper mapper,
                                  ICommunicationLogAppService CommunicationLogAppService, IBackgroundClearTaskSettings BackgroundClearTaskSettings)
        {
            _mapper = mapper;
            _CommunicationLogAppService = CommunicationLogAppService;
            _BackgroundClearTaskSettings = BackgroundClearTaskSettings;
        }
        public async Task ClearCommunicationLog()
        {
            try
            {
                var date = DateTime.Now.AddDays(-1 * int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearCommunicationLog)); ;
                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearCommunicationLog))
                {
                    await _CommunicationLogAppService.RemoveRange(date);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
