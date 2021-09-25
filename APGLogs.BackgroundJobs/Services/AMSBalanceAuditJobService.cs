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
    public class AMSBalanceAuditJobService : IAMSBalanceAuditJobService
    {
        private readonly IMapper _mapper;
        private readonly IAMSBalanceAuditAppService _AMSBalanceAuditAppService;
        private readonly IBackgroundClearTaskSettings _BackgroundClearTaskSettings;
        public AMSBalanceAuditJobService(IMapper mapper,
                                  IAMSBalanceAuditAppService AMSBalanceAuditAppService, IBackgroundClearTaskSettings BackgroundClearTaskSettings)
        {
            _mapper = mapper;
            _AMSBalanceAuditAppService = AMSBalanceAuditAppService;
            _BackgroundClearTaskSettings = BackgroundClearTaskSettings;
        }
        public async Task ClearAMSBalanceAudit()
        {
            try
            {
                var date = DateTime.Now.AddDays(-1 * int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearAMSBalanceAudit));
                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearAMSBalanceAudit))
                {
                    await _AMSBalanceAuditAppService.RemoveRange(date);
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
