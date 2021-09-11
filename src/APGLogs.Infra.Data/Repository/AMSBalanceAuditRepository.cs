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
    public class AMSBalanceAuditRepository : IAMSBalanceAuditRepository
    {
        private readonly IMongoCollection<AMSBalanceAudit> _AMSBalanceAudit;

        public AMSBalanceAuditRepository(IAPGLogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _AMSBalanceAudit = database.GetCollection<AMSBalanceAudit>(settings.AMSBalanceAuditCollectionName);
        }


        public async Task<AMSBalanceAudit> GetById(Guid id)
        {
            return await _AMSBalanceAudit.Find<AMSBalanceAudit>(x => x.Id == id.ToString()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AMSBalanceAudit>> GetAll()
        {
            List<AMSBalanceAudit> AMSBalanceAudits;
            AMSBalanceAudits = await _AMSBalanceAudit.Find(x => true).ToListAsync();
            return AMSBalanceAudits;
        }
        public async Task<PaginatedResult<AMSBalanceAudit>> GetPaginatedResultAsync(AMSBalanceAuditFilter filter)
        {
            FilterDefinition<AMSBalanceAudit> AMSBalanceAuditFilter = Builders<AMSBalanceAudit>.Filter.Empty;
            var dateTimeFieldDefinition = new ExpressionFieldDefinition<AMSBalanceAudit, DateTime>(x => x.CreationDate);
            var aMSBalanceIdDefinition = new ExpressionFieldDefinition<AMSBalanceAudit, Guid>(x => x.AMSBalanceId);
            var dateFrom = new DateTime();

            FilterDefinition<AMSBalanceAudit> aMSBalanceIdFilter = null;
            //if (filter.AMSBalanceId != null)
            //{
                aMSBalanceIdFilter = Builders<AMSBalanceAudit>.Filter.Eq(aMSBalanceIdDefinition, filter.AMSBalanceId);
                AMSBalanceAuditFilter = AMSBalanceAuditFilter & Builders<AMSBalanceAudit>.Filter.And(aMSBalanceIdFilter);
            //}


            FilterDefinition<AMSBalanceAudit> dateFromFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateFrom.ToString()) && filter.DateFrom > DateTime.MinValue)
            {
                filter.DateFrom = filter.DateFrom.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateFrom.Year, filter.DateFrom.Month, filter.DateFrom.Day,
                    filter.DateFrom.Hour, filter.DateFrom.Minute, filter.DateFrom.Second);
                dateFrom = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); ;
                dateFromFilter = Builders<AMSBalanceAudit>.Filter.Gte<DateTime>(dateTimeFieldDefinition, dateFrom);
                AMSBalanceAuditFilter = AMSBalanceAuditFilter & Builders<AMSBalanceAudit>.Filter.And(dateFromFilter);
            }

            var dateTo = new DateTime();
            FilterDefinition<AMSBalanceAudit> dateToFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateTo.ToString()) && filter.DateTo > DateTime.MinValue)
            {
                filter.DateTo = filter.DateTo.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateTo.Year, filter.DateTo.Month, filter.DateTo.Day,
                    filter.DateTo.Hour, filter.DateTo.Minute, filter.DateTo.Second);
                dateTo = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                dateToFilter = Builders<AMSBalanceAudit>.Filter.Lte<DateTime>(dateTimeFieldDefinition, dateTo);
                AMSBalanceAuditFilter = AMSBalanceAuditFilter & Builders<AMSBalanceAudit>.Filter.And(dateToFilter);
            }

            var sortDefinition = Builders<AMSBalanceAudit>.Sort.Descending(a => a.CreationDate);

            ProjectionDefinition<AMSBalanceAudit> project = Builders<AMSBalanceAudit>
                    .Projection.Include(x => x.Id);
            project.Include(x => x.AuditMessage);

            var queryCount = _AMSBalanceAudit.Find(AMSBalanceAuditFilter).Project(project).CountDocumentsAsync().Result;

            if (!filter.IsExport)
            {
                return new PaginatedResult<AMSBalanceAudit>
                {
                    Records = await _AMSBalanceAudit.Find(AMSBalanceAuditFilter).Sort(sortDefinition)
                    .Skip(Pager.Skip(filter.CurrentPage, filter.PageSize)).Limit(filter.PageSize).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
            else
            {
                return new PaginatedResult<AMSBalanceAudit>
                {
                    Records = await _AMSBalanceAudit.Find(AMSBalanceAuditFilter).Sort(sortDefinition).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
        }

        public Task Add(AMSBalanceAudit AMSBalanceAudit)
        {
            return _AMSBalanceAudit.InsertOneAsync(AMSBalanceAudit);
        }

        public Task Update(AMSBalanceAudit AMSBalanceAudit)
        {
            return _AMSBalanceAudit.ReplaceOneAsync(sub => sub.Id == AMSBalanceAudit.Id, AMSBalanceAudit);
        }

        public Task Remove(Guid id)
        {
            return _AMSBalanceAudit.DeleteOneAsync(sub => sub.Id == id.ToString());
        }

        public void Dispose()
        {
            //Db.Dispose();
        }
    }
}
