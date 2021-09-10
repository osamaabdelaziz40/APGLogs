using APGLogs.DomainHelper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGLogs.DomainHelper.Filter
{
    public class CommunicationLogFilter : PagingQueryWithExport
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string ServiceName { get; set; }
    }
}
