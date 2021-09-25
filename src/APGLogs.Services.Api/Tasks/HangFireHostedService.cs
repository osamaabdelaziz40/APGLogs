//using APGLogs.Application.Interfaces;
//using APGLogs.Domain.Interfaces;
//using APGLogs.Domain.Models;
//using Hangfire;
//using Microsoft.Extensions.Hosting;
//using MongoDB.Driver;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace APGLogs.Services.Api.Tasks
//{
//    public class HangFireHostedService : IHostedService
//    {
//        private readonly IBackgroundClearTaskSettings _BackgroundClearTaskSettings;
//        private readonly IAPGLogDatabaseSettings _APGLogDatabaseSettings;
//        private readonly IMongoCollection<CommunicationLog> _communicationLog;
//        private readonly IMongoCollection<ExceptionLog> _exceptionLog;
//        private readonly IMongoCollection<PortalSessionAudit> _portalSessionAudit;
//        private readonly IMongoCollection<PortalSessionAuditAction> _portalSessionAuditAction;
//        private readonly IMongoCollection<AMSTransactionAudit> _AMSTransactionAudit;
//        private readonly IMongoCollection<AMSBalanceAudit> _AMSBalanceAudit;
//        private readonly IMongoCollection<ShadowBalanceAudit> _ShadowBalanceAudit;
//        private readonly IMongoCollection<SMSLog> _SMSLog;
//        private readonly IMongoCollection<EmailLog> _EmailLog;


//        public HangFireHostedService(
//                IBackgroundClearTaskSettings BackgroundClearTaskSettings,
//                IAPGLogDatabaseSettings APGLogDatabaseSettings
//            )
//        {
//            var client = new MongoClient(APGLogDatabaseSettings.ConnectionString);

//            var database = client.GetDatabase(APGLogDatabaseSettings.DatabaseName);

//            _communicationLog = database.GetCollection<CommunicationLog>(APGLogDatabaseSettings.CommunicationLogCollectionName);
//            _exceptionLog = database.GetCollection<ExceptionLog>(APGLogDatabaseSettings.ExceptionLogCollectionName);
//            _portalSessionAudit = database.GetCollection<PortalSessionAudit>(APGLogDatabaseSettings.PortalSessionAuditCollectionName);
//            _portalSessionAuditAction = database.GetCollection<PortalSessionAuditAction>(APGLogDatabaseSettings.PortalSessionAuditActionCollectionName);
//            _AMSTransactionAudit = database.GetCollection<AMSTransactionAudit>(APGLogDatabaseSettings.AMSTransactionAuditCollectionName);
//            _AMSBalanceAudit = database.GetCollection<AMSBalanceAudit>(APGLogDatabaseSettings.AMSBalanceAuditCollectionName);
//            _ShadowBalanceAudit = database.GetCollection<ShadowBalanceAudit>(APGLogDatabaseSettings.ShadowBalanceAuditCollectionName);
//            _SMSLog = database.GetCollection<SMSLog>(APGLogDatabaseSettings.SMSLogCollectionName);
//            _EmailLog = database.GetCollection<EmailLog>(APGLogDatabaseSettings.EmailLogCollectionName);

//            _APGLogDatabaseSettings = APGLogDatabaseSettings;
//            _BackgroundClearTaskSettings = BackgroundClearTaskSettings;
//        }
//        public Task StartAsync(CancellationToken cancellationToken)
//        {

//            while (!cancellationToken.IsCancellationRequested)
//            {
//                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearExceptionLog))
//                {
//                    RecurringJob.AddOrUpdate(() => ClearExceptionLog(), Cron.DayInterval(int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearExceptionLog)));
//                }
//                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearCommunicationLog))
//                {
//                    RecurringJob.AddOrUpdate(() => ClearCommunicationLog(), Cron.DayInterval(int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearCommunicationLog)));
//                }
//                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearPortalSessionAudit))
//                {
//                    RecurringJob.AddOrUpdate(() => ClearPortalSessionAudit(), Cron.DayInterval(int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearPortalSessionAudit)));
//                }
//                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearPortalSessionAuditAction))
//                {
//                    RecurringJob.AddOrUpdate(() => ClearPortalSessionAuditAction(), Cron.DayInterval(int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearPortalSessionAuditAction)));
//                }
//                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearAMSBalanceAudit))
//                {
//                    RecurringJob.AddOrUpdate(() => ClearAMSBalanceAudit(), Cron.DayInterval(int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearAMSBalanceAudit)));
//                }
//                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearAMSTransactionAudit))
//                {
//                    RecurringJob.AddOrUpdate(() => ClearAMSTransactionAudit(), Cron.DayInterval(int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearAMSTransactionAudit)));
//                }
//                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearShadowBalanceAudit))
//                {
//                    RecurringJob.AddOrUpdate(() => ClearShadowBalanceAudit(), Cron.DayInterval(int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearShadowBalanceAudit)));
//                }
//                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearSMSLog))
//                {
//                    RecurringJob.AddOrUpdate(() => ClearSMSLog(), Cron.DayInterval(int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearSMSLog)));
//                }
//                if (!string.IsNullOrEmpty(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearEmailLog))
//                {
//                    RecurringJob.AddOrUpdate(() => ClearEmailLog(), Cron.DayInterval(int.Parse(_BackgroundClearTaskSettings.NumberOfIntervalDaysToClearEmailLog)));
//                }


//                return Task.CompletedTask;
//            }
//            return null;

//        }
//        public async Task ClearExceptionLog()
//        {
//            List<ExceptionLog> exceptionLogs;
//            exceptionLogs = await _exceptionLog.Find(x => true).ToListAsync();

//            foreach (var item in exceptionLogs)
//            {
//                await _exceptionLog.DeleteOneAsync(sub => sub.Id == item.Id.ToString());
//            }
//        }
//        public async Task ClearCommunicationLog()
//        {
//            List<CommunicationLog> communicationLogs;
//            communicationLogs = await _communicationLog.Find(x => true).ToListAsync();

//            foreach (var item in communicationLogs)
//            {
//                await _communicationLog.DeleteOneAsync(sub => sub.Id == item.Id.ToString());
//            }
//        }
//        public async Task ClearPortalSessionAudit()
//        {
//            List<PortalSessionAudit> portalSessionAudit;
//            portalSessionAudit = await _portalSessionAudit.Find(x => true).ToListAsync();

//            foreach (var item in portalSessionAudit)
//            {
//                await _portalSessionAudit.DeleteOneAsync(sub => sub.Id == item.Id.ToString());
//            }
//        }
//        public async Task ClearPortalSessionAuditAction()
//        {
//            List<PortalSessionAuditAction> portalSessionAuditAction;
//            portalSessionAuditAction = await _portalSessionAuditAction.Find(x => true).ToListAsync();

//            foreach (var item in portalSessionAuditAction)
//            {
//                await _portalSessionAuditAction.DeleteOneAsync(sub => sub.Id == item.Id.ToString());
//            }
//        }
//        public async Task ClearAMSTransactionAudit()
//        {
//            List<AMSTransactionAudit> AMSTransactionAudit;
//            AMSTransactionAudit = await _AMSTransactionAudit.Find(x => true).ToListAsync();

//            foreach (var item in AMSTransactionAudit)
//            {
//                await _AMSTransactionAudit.DeleteOneAsync(sub => sub.Id == item.Id.ToString());
//            }
//        }
//        public async Task ClearAMSBalanceAudit()
//        {
//            List<AMSBalanceAudit> AMSBalanceAudit;
//            AMSBalanceAudit = await _AMSBalanceAudit.Find(x => true).ToListAsync();

//            foreach (var item in AMSBalanceAudit)
//            {
//                await _AMSBalanceAudit.DeleteOneAsync(sub => sub.Id == item.Id.ToString());
//            }
//        }
//        public async Task ClearShadowBalanceAudit()
//        {
//            List<ShadowBalanceAudit> ShadowBalanceAudit;
//            ShadowBalanceAudit = await _ShadowBalanceAudit.Find(x => true).ToListAsync();

//            foreach (var item in ShadowBalanceAudit)
//            {
//                await _ShadowBalanceAudit.DeleteOneAsync(sub => sub.Id == item.Id.ToString());
//            }
//        }
//        public async Task ClearSMSLog()
//        {
//            List<SMSLog> SMSLog;
//            SMSLog = await _SMSLog.Find(x => true).ToListAsync();

//            foreach (var item in SMSLog)
//            {
//                await _SMSLog.DeleteOneAsync(sub => sub.Id == item.Id.ToString());
//            }
//        }
//        public async Task ClearEmailLog()
//        {
//            List<EmailLog> EmailLog;
//            EmailLog = await _EmailLog.Find(x => true).ToListAsync();

//            foreach (var item in EmailLog)
//            {
//                await _EmailLog.DeleteOneAsync(sub => sub.Id == item.Id.ToString());
//            }
//        }
//        public Task StopAsync(CancellationToken cancellationToken)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
