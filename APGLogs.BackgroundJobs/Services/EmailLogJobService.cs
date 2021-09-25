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
    public class EmailLogJobService : IEmailLogJobService
    {
        private readonly IMapper _mapper;
        private readonly IEmailLogAppService _EmailLogAppService;
        private readonly IBackgroundClearTaskSettings _BackgroundClearTaskSettings;
        public EmailLogJobService(IMapper mapper,
                                  IEmailLogAppService EmailLogAppService, IBackgroundClearTaskSettings BackgroundClearTaskSettings)
        {
            _mapper = mapper;
            _EmailLogAppService = EmailLogAppService;
            _BackgroundClearTaskSettings = BackgroundClearTaskSettings;
        }
        public async Task ClearEmailLog()
        {
            try
            {
                var date = DateTime.Now.AddDays(-1 * int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearEmailLog)); ;
                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearEmailLog))
                {
                    await _EmailLogAppService.RemoveRange(date);
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
