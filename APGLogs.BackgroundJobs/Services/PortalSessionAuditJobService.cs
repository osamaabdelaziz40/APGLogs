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
    public class PortalSessionAuditJobService : IPortalSessionAuditJobService
    {
        private readonly IMapper _mapper;
        private readonly IPortalSessionAuditAppService _PortalSessionAuditAppService;
        private readonly IBackgroundClearTaskSettings _BackgroundClearTaskSettings;
        public PortalSessionAuditJobService(IMapper mapper,
                                  IPortalSessionAuditAppService PortalSessionAuditAppService, IBackgroundClearTaskSettings BackgroundClearTaskSettings)
        {
            _mapper = mapper;
            _PortalSessionAuditAppService = PortalSessionAuditAppService;
            _BackgroundClearTaskSettings = BackgroundClearTaskSettings;
        }
        public async Task ClearPortalSessionAudit()
        {
            try
            {
                var date = DateTime.Now.AddDays(-1 * int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearPortalSessionAudit)); ;
                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearPortalSessionAudit))
                {
                    await _PortalSessionAuditAppService.RemoveRange(date);
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
