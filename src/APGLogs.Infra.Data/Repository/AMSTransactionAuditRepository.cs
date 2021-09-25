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
    public class AMSTransactionAuditRepository : IAMSTransactionAuditRepository
    {
        private readonly IMongoCollection<AMSTransactionAudit> _AMSTransactionAudit;

        public AMSTransactionAuditRepository(IAPGLogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _AMSTransactionAudit = database.GetCollection<AMSTransactionAudit>(settings.AMSTransactionAuditCollectionName);
        }


        public async Task<AMSTransactionAudit> GetById(Guid id)
        {
            //return await DbSet.FindAsync(id);
            return await _AMSTransactionAudit.Find<AMSTransactionAudit>(x => x.Id == id.ToString()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AMSTransactionAudit>> GetAll()
        {
            List<AMSTransactionAudit> AMSTransactionAudits;
            AMSTransactionAudits = await _AMSTransactionAudit.Find(x => true).ToListAsync();
            return AMSTransactionAudits;
        }
        public async Task<PaginatedResult<AMSTransactionAudit>> GetPaginatedResultAsync(AMSTransactionAuditFilter filter)
        {
            FilterDefinition<AMSTransactionAudit> AMSTransactionAuditFilter = Builders<AMSTransactionAudit>.Filter.Empty;
            var dateTimeFieldDefinition = new ExpressionFieldDefinition<AMSTransactionAudit, DateTime>(x => x.CreationDate);
            var dateFrom = new DateTime();
            FilterDefinition<AMSTransactionAudit> dateFromFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateFrom.ToString()) && filter.DateFrom > DateTime.MinValue)
            {
                filter.DateFrom = filter.DateFrom.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateFrom.Year, filter.DateFrom.Month, filter.DateFrom.Day,
                    filter.DateFrom.Hour, filter.DateFrom.Minute, filter.DateFrom.Second);
                dateFrom = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); ;
                dateFromFilter = Builders<AMSTransactionAudit>.Filter.Gte<DateTime>(dateTimeFieldDefinition, dateFrom);
                AMSTransactionAuditFilter = AMSTransactionAuditFilter & Builders<AMSTransactionAudit>.Filter.And(dateFromFilter);
            }

            var dateTo = new DateTime();
            FilterDefinition<AMSTransactionAudit> dateToFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateTo.ToString()) && filter.DateTo > DateTime.MinValue)
            {
                filter.DateTo = filter.DateTo.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateTo.Year, filter.DateTo.Month, filter.DateTo.Day,
                    filter.DateTo.Hour, filter.DateTo.Minute, filter.DateTo.Second);
                dateTo = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                dateToFilter = Builders<AMSTransactionAudit>.Filter.Lte<DateTime>(dateTimeFieldDefinition, dateTo);
                AMSTransactionAuditFilter = AMSTransactionAuditFilter & Builders<AMSTransactionAudit>.Filter.And(dateToFilter);
            }

            var sortDefinition = Builders<AMSTransactionAudit>.Sort.Descending(a => a.CreationDate);

            ProjectionDefinition<AMSTransactionAudit> project = Builders<AMSTransactionAudit>
                    .Projection.Include(x => x.Id);
            project.Include(x => x.AuditMessage);

            var queryCount = _AMSTransactionAudit.Find(AMSTransactionAuditFilter).Project(project).CountDocumentsAsync().Result;

            if (!filter.IsExport)
            {
                return new PaginatedResult<AMSTransactionAudit>
                {
                    Records = await _AMSTransactionAudit.Find(AMSTransactionAuditFilter).Sort(sortDefinition)
                    .Skip(Pager.Skip(filter.CurrentPage, filter.PageSize)).Limit(filter.PageSize).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
            else
            {
                return new PaginatedResult<AMSTransactionAudit>
                {
                    Records = await _AMSTransactionAudit.Find(AMSTransactionAuditFilter).Sort(sortDefinition).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
        }

        public Task Add(AMSTransactionAudit AMSTransactionAudit)
        {
            return _AMSTransactionAudit.InsertOneAsync(AMSTransactionAudit);
        }

        public Task Update(AMSTransactionAudit AMSTransactionAudit)
        {
            return _AMSTransactionAudit.ReplaceOneAsync(sub => sub.Id == AMSTransactionAudit.Id, AMSTransactionAudit);
        }

        public Task Remove(Guid id)
        {
            return _AMSTransactionAudit.DeleteOneAsync(sub => sub.Id == id.ToString());
        }

        public Task RemoveRange(DateTime date)
        {
            FilterDefinition<AMSTransactionAudit> AMSTransactionAuditFilter = Builders<AMSTransactionAudit>.Filter.Empty;
            var dateTimeFieldDefinition = new ExpressionFieldDefinition<AMSTransactionAudit, DateTime>(x => x.CreationDate);
            var dateFrom = new DateTime();
            FilterDefinition<AMSTransactionAudit> dateFromFilter = null;
            if (!string.IsNullOrWhiteSpace(date.ToString()))
            {
                date = date.ToLocalTime();
                var convertedDateTime = new DateTime(date.Year, date.Month, date.Day,
                    date.Hour, date.Minute, date.Second);
                dateFrom = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); ;
                dateFromFilter = Builders<AMSTransactionAudit>.Filter.Lte<DateTime>(dateTimeFieldDefinition, dateFrom);
                AMSTransactionAuditFilter = AMSTransactionAuditFilter & Builders<AMSTransactionAudit>.Filter.And(dateFromFilter);
            }
            return _AMSTransactionAudit.DeleteManyAsync(AMSTransactionAuditFilter);
        }
        public void Dispose()
        {
            //Db.Dispose();
        }
    }
}
