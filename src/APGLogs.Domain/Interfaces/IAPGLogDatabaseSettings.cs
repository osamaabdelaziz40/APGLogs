using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGLogs.Domain.Interfaces
{
    public interface IAPGLogDatabaseSettings
    {
        public string ExceptionLogCollectionName { get; set; }
        public string ExceptionLogTypeCollectionName { get; set; }
        public string CommunicationLogCollectionName { get; set; }
        public string SMSLogCollectionName { get; set; }
        public string PortalSessionAuditCollectionName { get; set; }
        public string PortalSessionAuditActionCollectionName { get; set; }
        public string AMSTransactionAuditCollectionName { get; set; }
        public string AMSBalanceAuditCollectionName { get; set; }
        public string ShadowBalanceAuditCollectionName { get; set; }
        public string EmailLogCollectionName { get; set; }
        public string CommunicationLogTypeCollectionName { get; set; }
        public string BalanceAndAMSAuditsCollectionName { get; set; }
        public string PortalAuditsCollectionCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
