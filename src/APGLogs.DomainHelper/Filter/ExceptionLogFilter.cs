using APGLogs.DomainHelper.Attributes;
using System;

namespace APGLogs.DomainHelper.Filter
{
    public class ExceptionLogFilter : PagingQueryWithExport
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Guid CommunicationLogId { get; set; }
        public string ExceptionType { get; set; }
    }
}
