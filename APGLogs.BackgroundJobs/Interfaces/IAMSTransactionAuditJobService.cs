using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGLogs.BackgroundJobs.Interfaces
{
    public interface IAMSTransactionAuditJobService : IDisposable
    {
        public Task ClearAMSTransactionAudit();
    }
}
