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
    public class PortalSessionAuditActionJobService : IPortalSessionAuditActionJobService
    {
        private readonly IMapper _mapper;
        private readonly IPortalSessionAuditActionAppService _PortalSessionAuditActionAppService;
        private readonly IBackgroundClearTaskSettings _BackgroundClearTaskSettings;
        public PortalSessionAuditActionJobService(IMapper mapper,
                                  IPortalSessionAuditActionAppService PortalSessionAuditActionAppService, IBackgroundClearTaskSettings BackgroundClearTaskSettings)
        {
            _mapper = mapper;
            _PortalSessionAuditActionAppService = PortalSessionAuditActionAppService;
            _BackgroundClearTaskSettings = BackgroundClearTaskSettings;
        }
        public async Task ClearPortalSessionAuditAction()
        {
            try
            {
                var date = DateTime.Now.AddDays(-1 * int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearPortalSessionAuditAction)); ;
                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearPortalSessionAuditAction))
                {
                    await _PortalSessionAuditActionAppService.RemoveRange(date);
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
