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
    public class ShadowBalanceAuditJobService : IShadowBalanceAuditJobService
    {
        private readonly IMapper _mapper;
        private readonly IShadowBalanceAuditAppService _ShadowBalanceAuditAppService;
        private readonly IBackgroundClearTaskSettings _BackgroundClearTaskSettings;
        public ShadowBalanceAuditJobService(IMapper mapper,
                                  IShadowBalanceAuditAppService ShadowBalanceAuditAppService, IBackgroundClearTaskSettings BackgroundClearTaskSettings)
        {
            _mapper = mapper;
            _ShadowBalanceAuditAppService = ShadowBalanceAuditAppService;
            _BackgroundClearTaskSettings = BackgroundClearTaskSettings;
        }
        public async Task ClearShadowBalanceAudit()
        {
            try
            {
                var date = DateTime.Now.AddDays(-1 * int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearShadowBalanceAudit)); ;
                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearShadowBalanceAudit))
                {
                    await _ShadowBalanceAuditAppService.RemoveRange(date);
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
