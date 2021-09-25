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
    public class ShadowBalanceAuditRepository : IShadowBalanceAuditRepository
    {
        private readonly IMongoCollection<ShadowBalanceAudit> _ShadowBalanceAudit;

        public ShadowBalanceAuditRepository(IAPGLogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _ShadowBalanceAudit = database.GetCollection<ShadowBalanceAudit>(settings.ShadowBalanceAuditCollectionName);
        }


        public async Task<ShadowBalanceAudit> GetById(Guid id)
        {
            //return await DbSet.FindAsync(id);
            return await _ShadowBalanceAudit.Find<ShadowBalanceAudit>(x => x.Id == id.ToString()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ShadowBalanceAudit>> GetAll()
        {
            List<ShadowBalanceAudit> ShadowBalanceAudits;
            ShadowBalanceAudits = await _ShadowBalanceAudit.Find(x => true).ToListAsync();
            return ShadowBalanceAudits;
        }
        public async Task<PaginatedResult<ShadowBalanceAudit>> GetPaginatedResultAsync(ShadowBalanceAuditFilter filter)
        {
            FilterDefinition<ShadowBalanceAudit> ShadowBalanceAuditFilter = Builders<ShadowBalanceAudit>.Filter.Empty;
            var dateTimeFieldDefinition = new ExpressionFieldDefinition<ShadowBalanceAudit, DateTime>(x => x.CreationDate);

            var shadowBalanceIdDefinition = new ExpressionFieldDefinition<ShadowBalanceAudit, Guid>(x => x.ShadowBalanceId);
            
            FilterDefinition<ShadowBalanceAudit> shadowBalanceIdFilter = null;
            //if (filter.ShadowBalanceId != null)
            //{
                shadowBalanceIdFilter = Builders<ShadowBalanceAudit>.Filter.Eq(shadowBalanceIdDefinition, filter.ShadowBalanceId);
                ShadowBalanceAuditFilter = ShadowBalanceAuditFilter & Builders<ShadowBalanceAudit>.Filter.And(shadowBalanceIdFilter);
            //}

            var dateFrom = new DateTime();


            FilterDefinition<ShadowBalanceAudit> dateFromFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateFrom.ToString()) && filter.DateFrom > DateTime.MinValue)
            {
                filter.DateFrom = filter.DateFrom.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateFrom.Year, filter.DateFrom.Month, filter.DateFrom.Day,
                    filter.DateFrom.Hour, filter.DateFrom.Minute, filter.DateFrom.Second);
                dateFrom = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); ;
                dateFromFilter = Builders<ShadowBalanceAudit>.Filter.Gte<DateTime>(dateTimeFieldDefinition, dateFrom);
                ShadowBalanceAuditFilter = ShadowBalanceAuditFilter & Builders<ShadowBalanceAudit>.Filter.And(dateFromFilter);
            }

            var dateTo = new DateTime();
            FilterDefinition<ShadowBalanceAudit> dateToFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateTo.ToString()) && filter.DateTo > DateTime.MinValue)
            {
                filter.DateTo = filter.DateTo.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateTo.Year, filter.DateTo.Month, filter.DateTo.Day,
                    filter.DateTo.Hour, filter.DateTo.Minute, filter.DateTo.Second);
                dateTo = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                dateToFilter = Builders<ShadowBalanceAudit>.Filter.Lte<DateTime>(dateTimeFieldDefinition, dateTo);
                ShadowBalanceAuditFilter = ShadowBalanceAuditFilter & Builders<ShadowBalanceAudit>.Filter.And(dateToFilter);
            }

            var sortDefinition = Builders<ShadowBalanceAudit>.Sort.Descending(a => a.CreationDate);

            ProjectionDefinition<ShadowBalanceAudit> project = Builders<ShadowBalanceAudit>
                    .Projection.Include(x => x.Id);
            project.Include(x => x.AuditMessage);

            var queryCount = _ShadowBalanceAudit.Find(ShadowBalanceAuditFilter).Project(project).CountDocumentsAsync().Result;

            if (!filter.IsExport)
            {
                return new PaginatedResult<ShadowBalanceAudit>
                {
                    Records = await _ShadowBalanceAudit.Find(ShadowBalanceAuditFilter).Sort(sortDefinition)
                    .Skip(Pager.Skip(filter.CurrentPage, filter.PageSize)).Limit(filter.PageSize).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
            else
            {
                return new PaginatedResult<ShadowBalanceAudit>
                {
                    Records = await _ShadowBalanceAudit.Find(ShadowBalanceAuditFilter).Sort(sortDefinition).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
        }

        public Task Add(ShadowBalanceAudit ShadowBalanceAudit)
        {
            return _ShadowBalanceAudit.InsertOneAsync(ShadowBalanceAudit);
        }

        public Task Update(ShadowBalanceAudit ShadowBalanceAudit)
        {
            return _ShadowBalanceAudit.ReplaceOneAsync(sub => sub.Id == ShadowBalanceAudit.Id, ShadowBalanceAudit);
        }

        public Task Remove(Guid id)
        {
            return _ShadowBalanceAudit.DeleteOneAsync(sub => sub.Id == id.ToString());
        }
        public Task RemoveRange(DateTime date)
        {
            FilterDefinition<ShadowBalanceAudit> ShadowBalanceAuditFilter = Builders<ShadowBalanceAudit>.Filter.Empty;
            var dateTimeFieldDefinition = new ExpressionFieldDefinition<ShadowBalanceAudit, DateTime>(x => x.CreationDate);
            var dateFrom = new DateTime();
            FilterDefinition<ShadowBalanceAudit> dateFromFilter = null;
            if (!string.IsNullOrWhiteSpace(date.ToString()))
            {
                date = date.ToLocalTime();
                var convertedDateTime = new DateTime(date.Year, date.Month, date.Day,
                    date.Hour, date.Minute, date.Second);
                dateFrom = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); ;
                dateFromFilter = Builders<ShadowBalanceAudit>.Filter.Lte<DateTime>(dateTimeFieldDefinition, dateFrom);
                ShadowBalanceAuditFilter = ShadowBalanceAuditFilter & Builders<ShadowBalanceAudit>.Filter.And(dateFromFilter);
            }
            return _ShadowBalanceAudit.DeleteManyAsync(ShadowBalanceAuditFilter);
        }

        public void Dispose()
        {
            //Db.Dispose();
        }
    }
}
