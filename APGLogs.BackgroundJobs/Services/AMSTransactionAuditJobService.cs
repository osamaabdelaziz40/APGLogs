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
    public class AMSTransactionAuditJobService : IAMSTransactionAuditJobService
    {
        private readonly IMapper _mapper;
        private readonly IAMSTransactionAuditAppService _AMSTransactionAuditAppService;
        private readonly IBackgroundClearTaskSettings _BackgroundClearTaskSettings;
        public AMSTransactionAuditJobService(IMapper mapper,
                                  IAMSTransactionAuditAppService AMSTransactionAuditAppService, IBackgroundClearTaskSettings BackgroundClearTaskSettings)
        {
            _mapper = mapper;
            _AMSTransactionAuditAppService = AMSTransactionAuditAppService;
            _BackgroundClearTaskSettings = BackgroundClearTaskSettings;
        }
        public async Task ClearAMSTransactionAudit()
        {
            try
            {
                var date = DateTime.Now.AddDays(-1 * int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearAMSTransactionAudit)); ;
                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearAMSTransactionAudit))
                {
                    await _AMSTransactionAuditAppService.RemoveRange(date);
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
