using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGLogs.BackgroundJobs.Interfaces
{
    public interface IPortalSessionAuditJobService : IDisposable
    {
        public Task ClearPortalSessionAudit();
    }
}
