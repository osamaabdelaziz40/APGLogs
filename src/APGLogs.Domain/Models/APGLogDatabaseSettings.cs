using APGLogs.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGLogs.Domain.Models
{
    public class APGLogDatabaseSettings : IAPGLogDatabaseSettings
    {
        public const string APGLogDatabaseSettingsProps = "APGLogDatabaseSettings";

        public string ExceptionLogCollectionName { get; set; }
        public string CommunicationLogCollectionName { get; set; }
        public string BalanceAndAMSAuditsCollectionName { get; set; }
        public string PortalAuditsCollectionCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
