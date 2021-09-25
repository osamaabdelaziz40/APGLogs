using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using APGFundamentals.DomainHelper.Services;
using APGLogs.Domain.Interfaces;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Filter;
using APGLogs.DomainHelper.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace APGLogs.Infra.Data.Repository
{
    public class ExceptionLogRepository : IExceptionLogRepository
    {
        private readonly IMongoCollection<ExceptionLog> _exceptionLog;

        public ExceptionLogRepository(IAPGLogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _exceptionLog = database.GetCollection<ExceptionLog>(settings.ExceptionLogCollectionName);
        }

        public async Task<ExceptionLog> GetById(Guid id)
        {
            //return await DbSet.FindAsync(id);
            return await _exceptionLog.Find<ExceptionLog>(x => x.Id == id.ToString()).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<ExceptionLog>> GetByIdCommunicationLogId(Guid communicationLogId)
        {
            List<ExceptionLog> exceptionLogs;
            exceptionLogs = await _exceptionLog.Find(x => x.CommunicationLogId == communicationLogId.ToString()).ToListAsync();
            return exceptionLogs;
        }

        public async Task<IEnumerable<ExceptionLog>> GetAll()
        {
            List<ExceptionLog> exceptionLogs;
            exceptionLogs = await _exceptionLog.Find(x => true).ToListAsync();
            return exceptionLogs;
        }

        public async Task<PaginatedResult<ExceptionLog>> GetPaginatedResultAsync(ExceptionLogFilter filter)
        {
            FilterDefinition<ExceptionLog> exceptionLogFilter = Builders<ExceptionLog>.Filter.Empty;
            var exceptionTypeFieldDefinition = new ExpressionFieldDefinition<ExceptionLog, string>(x => x.ExceptionType);
            var dateTimeFieldDefinition = new ExpressionFieldDefinition<ExceptionLog, DateTime>(x => x.DateTime);

            FilterDefinition<ExceptionLog> exceptionTypeFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.ExceptionType))
            {
                exceptionTypeFilter = Builders<ExceptionLog>.Filter.Eq(exceptionTypeFieldDefinition, filter.ExceptionType);
                exceptionLogFilter = exceptionLogFilter & Builders<ExceptionLog>.Filter.And(exceptionTypeFilter);
            }

            var dateFrom = new DateTime();
            FilterDefinition<ExceptionLog> dateFromFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateFrom.ToString()) && filter.DateFrom > DateTime.MinValue)
            {
                filter.DateFrom = filter.DateFrom.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateFrom.Year, filter.DateFrom.Month, filter.DateFrom.Day,
                    filter.DateFrom.Hour, filter.DateFrom.Minute, filter.DateFrom.Second);
                dateFrom = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); ;
                dateFromFilter = Builders<ExceptionLog>.Filter.Gte<DateTime>(dateTimeFieldDefinition, dateFrom);
                exceptionLogFilter = exceptionLogFilter & Builders<ExceptionLog>.Filter.And(dateFromFilter);
            }

            var dateTo = new DateTime();
            FilterDefinition<ExceptionLog> dateToFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateTo.ToString()) && filter.DateTo > DateTime.MinValue)
            {
                filter.DateTo = filter.DateTo.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateTo.Year, filter.DateTo.Month, filter.DateTo.Day,
                    filter.DateTo.Hour, filter.DateTo.Minute, filter.DateTo.Second);
                dateTo = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                dateToFilter = Builders<ExceptionLog>.Filter.Lte<DateTime>(dateTimeFieldDefinition, dateTo);
                exceptionLogFilter = exceptionLogFilter & Builders<ExceptionLog>.Filter.And(dateToFilter);
            }

            var sortDefinition = Builders<ExceptionLog>.Sort.Descending(a => a.DateTime);

            ProjectionDefinition<ExceptionLog> project = Builders<ExceptionLog>
                    .Projection.Include(x => x.Id);
            project.Include(x => x.InnerException);
            project.Include(x => x.DateTime);

            var queryCount = _exceptionLog.Find(exceptionLogFilter).Project(project).CountDocumentsAsync().Result;

            if (!filter.IsExport)
            {
                return new PaginatedResult<ExceptionLog>
                {
                    Records = await _exceptionLog.Find(exceptionLogFilter).Sort(sortDefinition)
                    .Skip(Pager.Skip(filter.CurrentPage, filter.PageSize)).Limit(filter.PageSize).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
            else
            {
                return new PaginatedResult<ExceptionLog>
                {
                    Records = await _exceptionLog.Find(exceptionLogFilter).Sort(sortDefinition).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
        }

        public Task Add(ExceptionLog exceptionLog)
        {
            return _exceptionLog.InsertOneAsync(exceptionLog);
        }

        public Task Update(ExceptionLog exceptionLog)
        {
            return _exceptionLog.ReplaceOneAsync(sub => sub.Id == exceptionLog.Id, exceptionLog);
        }

        public Task Remove(Guid id)
        {
            return _exceptionLog.DeleteOneAsync(sub => sub.Id == id.ToString());
        }

        public Task RemoveRange(DateTime date)
        {
            FilterDefinition<ExceptionLog> exceptionLogFilter = Builders<ExceptionLog>.Filter.Empty;
            var dateTimeFieldDefinition = new ExpressionFieldDefinition<ExceptionLog, DateTime>(x => x.DateTime);
            var dateFrom = new DateTime();
            FilterDefinition<ExceptionLog> dateFromFilter = null;
            if (!string.IsNullOrWhiteSpace(date.ToString()))
            {
                date = date.ToLocalTime();
                var convertedDateTime = new DateTime(date.Year, date.Month, date.Day,
                    date.Hour, date.Minute, date.Second);
                dateFrom = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); ;
                dateFromFilter = Builders<ExceptionLog>.Filter.Lte<DateTime>(dateTimeFieldDefinition, dateFrom);
                exceptionLogFilter = exceptionLogFilter & Builders<ExceptionLog>.Filter.And(dateFromFilter);
            }
            return _exceptionLog.DeleteManyAsync(exceptionLogFilter);
        }
        public void Dispose()
        {
            //Db.Dispose();
        }
    }
}
