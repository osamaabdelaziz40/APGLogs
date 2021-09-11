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
    public class EmailLogRepository : IEmailLogRepository
    {
        private readonly IMongoCollection<EmailLog> _EmailLog;

        public EmailLogRepository(IAPGLogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _EmailLog = database.GetCollection<EmailLog>(settings.EmailLogCollectionName);
        }

        
        public async Task<EmailLog> GetById(Guid id)
        {
            //return await DbSet.FindAsync(id);
            return await _EmailLog.Find<EmailLog>(x => x.Id == id.ToString()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EmailLog>> GetAll()
        {
            List<EmailLog> EmailLogs;
            EmailLogs = await _EmailLog.Find(x => true).ToListAsync();
            return EmailLogs;
        }

        public async Task<PaginatedResult<EmailLog>> GetPaginatedResultAsync(EmailLogFilter filter)
        {
            FilterDefinition<EmailLog> EmailLogFilter = Builders<EmailLog>.Filter.Empty;
            var statusFieldDefinition = new ExpressionFieldDefinition<EmailLog, string>(x => x.Status);
            var dateTimeFieldDefinition = new ExpressionFieldDefinition<EmailLog, DateTime>(x => x.EmailDateTime);

            FilterDefinition<EmailLog> statusFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                statusFilter = Builders<EmailLog>.Filter.Eq(statusFieldDefinition, filter.Status);
                EmailLogFilter = EmailLogFilter & Builders<EmailLog>.Filter.And(statusFilter);
            }

            var dateFrom = new DateTime();
            FilterDefinition<EmailLog> dateFromFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateFrom.ToString()) && filter.DateFrom > DateTime.MinValue)
            {
                filter.DateFrom = filter.DateFrom.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateFrom.Year, filter.DateFrom.Month, filter.DateFrom.Day,
                    filter.DateFrom.Hour, filter.DateFrom.Minute, filter.DateFrom.Second);
                dateFrom = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); ;
                dateFromFilter = Builders<EmailLog>.Filter.Gte<DateTime>(dateTimeFieldDefinition, dateFrom);
                EmailLogFilter = EmailLogFilter & Builders<EmailLog>.Filter.And(dateFromFilter);
            }

            var dateTo = new DateTime();
            FilterDefinition<EmailLog> dateToFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateTo.ToString()) && filter.DateTo > DateTime.MinValue)
            {
                filter.DateTo = filter.DateTo.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateTo.Year, filter.DateTo.Month, filter.DateTo.Day,
                    filter.DateTo.Hour, filter.DateTo.Minute, filter.DateTo.Second);
                dateTo = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                dateToFilter = Builders<EmailLog>.Filter.Lte<DateTime>(dateTimeFieldDefinition, dateTo);
                EmailLogFilter = EmailLogFilter & Builders<EmailLog>.Filter.And(dateToFilter);
            }

            var sortDefinition = Builders<EmailLog>.Sort.Descending(a => a.EmailDateTime);

            ProjectionDefinition<EmailLog> project = Builders<EmailLog>
                    .Projection.Include(x => x.Id);
            project.Include(x => x.ToEmail);
            project.Include(x => x.Status);

            var queryCount = _EmailLog.Find(EmailLogFilter).Project(project).CountDocumentsAsync().Result;

            if (!filter.IsExport)
            {
                return new PaginatedResult<EmailLog>
                {
                    Records = await _EmailLog.Find(EmailLogFilter).Sort(sortDefinition)
                    .Skip(Pager.Skip(filter.CurrentPage, filter.PageSize)).Limit(filter.PageSize).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
            else
            {
                return new PaginatedResult<EmailLog>
                {
                    Records = await _EmailLog.Find(EmailLogFilter).Sort(sortDefinition).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
        }


        public Task Add(EmailLog EmailLog)
        {
            return _EmailLog.InsertOneAsync(EmailLog);
        }

        public Task Update(EmailLog EmailLog)
        {
            return _EmailLog.ReplaceOneAsync(sub => sub.Id == EmailLog.Id, EmailLog);
        }

        public Task Remove(Guid id)
        {
            return _EmailLog.DeleteOneAsync(sub => sub.Id == id.ToString());
        }

        public void Dispose()
        {
            //Db.Dispose();
        }
    }
}
