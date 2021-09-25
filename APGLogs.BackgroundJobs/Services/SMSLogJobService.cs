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
    public class SMSLogJobService : ISMSLogJobService
    {
        private readonly IMapper _mapper;
        private readonly ISMSLogAppService _SMSLogAppService;
        private readonly IBackgroundClearTaskSettings _BackgroundClearTaskSettings;
        public SMSLogJobService(IMapper mapper,
                                  ISMSLogAppService SMSLogAppService, IBackgroundClearTaskSettings BackgroundClearTaskSettings)
        {
            _mapper = mapper;
            _SMSLogAppService = SMSLogAppService;
            _BackgroundClearTaskSettings = BackgroundClearTaskSettings;
        }
        public async Task ClearSMSLog()
        {
            try
            {
                var date = DateTime.Now.AddDays(-1 * int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearSMSLog)); ;
                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearSMSLog))
                {
                    await _SMSLogAppService.RemoveRange(date);
                }
            }
            catch
            {
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
