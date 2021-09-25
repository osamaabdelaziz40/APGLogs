using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using APGFundamentals.DomainHelper.Services;
using APGLogs.Domain.Interfaces;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace APGLogs.Infra.Data.Repository
{
    public class SMSLogRepository : ISMSLogRepository
    {
        private readonly IMongoCollection<SMSLog> _SMSLog;

        public SMSLogRepository(IAPGLogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _SMSLog = database.GetCollection<SMSLog>(settings.SMSLogCollectionName);
        }

        
        public async Task<SMSLog> GetById(Guid id)
        {
            //return await DbSet.FindAsync(id);
            return await _SMSLog.Find<SMSLog>(x => x.Id == id.ToString()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SMSLog>> GetAll()
        {
            List<SMSLog> SMSLogs;
            SMSLogs = await _SMSLog.Find(x => true).ToListAsync();
            return SMSLogs;
        }

        public async Task<PaginatedResult<SMSLog>> GetPaginatedResultAsync(SMSLogFilter filter)
        {
            FilterDefinition<SMSLog> SMSLogFilter = Builders<SMSLog>.Filter.Empty;
            var statusFieldDefinition = new ExpressionFieldDefinition<SMSLog, string>(x => x.Status);
            var dateTimeFieldDefinition = new ExpressionFieldDefinition<SMSLog, DateTime>(x => x.DateTime);

            FilterDefinition<SMSLog> statusFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                statusFilter = Builders<SMSLog>.Filter.Eq(statusFieldDefinition, filter.Status);
                SMSLogFilter = SMSLogFilter & Builders<SMSLog>.Filter.And(statusFilter);
            }

            var dateFrom = new DateTime();
            FilterDefinition<SMSLog> dateFromFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateFrom.ToString()) && filter.DateFrom > DateTime.MinValue)
            {
                filter.DateFrom = filter.DateFrom.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateFrom.Year, filter.DateFrom.Month, filter.DateFrom.Day,
                    filter.DateFrom.Hour, filter.DateFrom.Minute, filter.DateFrom.Second);
                dateFrom = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); ;
                dateFromFilter = Builders<SMSLog>.Filter.Gte<DateTime>(dateTimeFieldDefinition, dateFrom);
                SMSLogFilter = SMSLogFilter & Builders<SMSLog>.Filter.And(dateFromFilter);
            }

            var dateTo = new DateTime();
            FilterDefinition<SMSLog> dateToFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateTo.ToString()) && filter.DateTo > DateTime.MinValue)
            {
                filter.DateTo = filter.DateTo.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateTo.Year, filter.DateTo.Month, filter.DateTo.Day,
                    filter.DateTo.Hour, filter.DateTo.Minute, filter.DateTo.Second);
                dateTo = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                dateToFilter = Builders<SMSLog>.Filter.Lte<DateTime>(dateTimeFieldDefinition, dateTo);
                SMSLogFilter = SMSLogFilter & Builders<SMSLog>.Filter.And(dateToFilter);
            }

            var sortDefinition = Builders<SMSLog>.Sort.Descending(a => a.DateTime);

            ProjectionDefinition<SMSLog> project = Builders<SMSLog>
                    .Projection.Include(x => x.Id);
            project.Include(x => x.SMS);
            project.Include(x => x.Status);
            project.Include(x => x.ToMobile);

            var queryCount = _SMSLog.Find(SMSLogFilter).Project(project).CountDocumentsAsync().Result;

            if (!filter.IsExport)
            {
                return new PaginatedResult<SMSLog>
                {
                    Records = await _SMSLog.Find(SMSLogFilter).Sort(sortDefinition)
                    .Skip(Pager.Skip(filter.CurrentPage, filter.PageSize)).Limit(filter.PageSize).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
            else
            {
                return new PaginatedResult<SMSLog>
                {
                    Records = await _SMSLog.Find(SMSLogFilter).Sort(sortDefinition).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
        }


        public Task Add(SMSLog SMSLog)
        {
            return _SMSLog.InsertOneAsync(SMSLog);
        }

        public Task Update(SMSLog SMSLog)
        {
            return _SMSLog.ReplaceOneAsync(sub => sub.Id == SMSLog.Id, SMSLog);
        }

        public Task Remove(Guid id)
        {
            return _SMSLog.DeleteOneAsync(sub => sub.Id == id.ToString());
        }

        public Task RemoveRange(DateTime date)
        {
            FilterDefinition<SMSLog> SMSLogFilter = Builders<SMSLog>.Filter.Empty;
            var dateTimeFieldDefinition = new ExpressionFieldDefinition<SMSLog, DateTime>(x => x.DateTime);
            var dateFrom = new DateTime();
            FilterDefinition<SMSLog> dateFromFilter = null;
            if (!string.IsNullOrWhiteSpace(date.ToString()))
            {
                date = date.ToLocalTime();
                var convertedDateTime = new DateTime(date.Year, date.Month, date.Day,
                    date.Hour, date.Minute, date.Second);
                dateFrom = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); ;
                dateFromFilter = Builders<SMSLog>.Filter.Lte<DateTime>(dateTimeFieldDefinition, dateFrom);
                SMSLogFilter = SMSLogFilter & Builders<SMSLog>.Filter.And(dateFromFilter);
            }
            return _SMSLog.DeleteManyAsync(SMSLogFilter);
        }

        public void Dispose()
        {
            //Db.Dispose();
        }
    }
}
