using APGLogs.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGLogs.Domain.Models
{
    public class BackgroundClearTaskSettings: IBackgroundClearTaskSettings
    {
        public const string BackgroundClearTaskSettingsProps = "BackgroundClearTaskSettings";

        public string NumberOfIntervalDaysToClearExceptionLog { get; set; }
        public string NumberOfIntervalDaysToClearCommunicationLog { get; set; }
        public string NumberOfIntervalDaysToClearPortalSessionAudit { get; set; }
        public string NumberOfIntervalDaysToClearPortalSessionAuditAction { get; set; }
        public string NumberOfIntervalDaysToClearAMSTransactionAudit { get; set; }
        public string NumberOfIntervalDaysToClearAMSBalanceAudit { get; set; }
        public string NumberOfIntervalDaysToClearShadowBalanceAudit { get; set; }
        public string NumberOfIntervalDaysToClearSMSLog { get; set; }
        public string NumberOfIntervalDaysToClearEmailLog { get; set; }
        public bool UseBase64Encoding { get; set; }
    }
}
