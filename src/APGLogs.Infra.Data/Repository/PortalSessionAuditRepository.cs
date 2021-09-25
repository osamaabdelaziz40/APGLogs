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
    public class PortalSessionAuditRepository : IPortalSessionAuditRepository
    {
        private readonly IMongoCollection<PortalSessionAudit> _PortalSessionAudit;

        public PortalSessionAuditRepository(IAPGLogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _PortalSessionAudit = database.GetCollection<PortalSessionAudit>(settings.PortalSessionAuditCollectionName);
        }

        
        public async Task<PortalSessionAudit> GetById(Guid id)
        {
            //return await DbSet.FindAsync(id);
            return await _PortalSessionAudit.Find<PortalSessionAudit>(x => x.Id == id.ToString()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PortalSessionAudit>> GetAll()
        {
            List<PortalSessionAudit> PortalSessionAudits;
            PortalSessionAudits = await _PortalSessionAudit.Find(x => true).ToListAsync();
            return PortalSessionAudits;
        }

        public async Task<PaginatedResult<PortalSessionAudit>> GetPaginatedResultAsync(PortalSessionAuditFilter filter)
        {
            FilterDefinition<PortalSessionAudit> PortalSessionAuditFilter = Builders<PortalSessionAudit>.Filter.Empty;
            //var statusFieldDefinition = new ExpressionFieldDefinition<PortalSessionAudit, string>(x => x.Status);
            var dateTimeFieldDefinition = new ExpressionFieldDefinition<PortalSessionAudit, DateTime>(x => x.DateTime);

            FilterDefinition<PortalSessionAudit> statusFilter = null;
           
            var dateFrom = new DateTime();
            FilterDefinition<PortalSessionAudit> dateFromFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateFrom.ToString()) && filter.DateFrom > DateTime.MinValue)
            {
                filter.DateFrom = filter.DateFrom.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateFrom.Year, filter.DateFrom.Month, filter.DateFrom.Day,
                    filter.DateFrom.Hour, filter.DateFrom.Minute, filter.DateFrom.Second);
                dateFrom = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); ;
                dateFromFilter = Builders<PortalSessionAudit>.Filter.Gte<DateTime>(dateTimeFieldDefinition, dateFrom);
                PortalSessionAuditFilter = PortalSessionAuditFilter & Builders<PortalSessionAudit>.Filter.And(dateFromFilter);
            }

            var dateTo = new DateTime();
            FilterDefinition<PortalSessionAudit> dateToFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateTo.ToString()) && filter.DateTo > DateTime.MinValue)
            {
                filter.DateTo = filter.DateTo.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateTo.Year, filter.DateTo.Month, filter.DateTo.Day,
                    filter.DateTo.Hour, filter.DateTo.Minute, filter.DateTo.Second);
                dateTo = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                dateToFilter = Builders<PortalSessionAudit>.Filter.Lte<DateTime>(dateTimeFieldDefinition, dateTo);
                PortalSessionAuditFilter = PortalSessionAuditFilter & Builders<PortalSessionAudit>.Filter.And(dateToFilter);
            }

            var sortDefinition = Builders<PortalSessionAudit>.Sort.Descending(a => a.DateTime);

            ProjectionDefinition<PortalSessionAudit> project = Builders<PortalSessionAudit>
                    .Projection.Include(x => x.Id);
            project.Include(x => x.IPAddress);
            project.Include(x => x.SessionID);
            project.Include(x => x.UserName);

            var queryCount = _PortalSessionAudit.Find(PortalSessionAuditFilter).Project(project).CountDocumentsAsync().Result;

            if (!filter.IsExport)
            {
                return new PaginatedResult<PortalSessionAudit>
                {
                    Records = await _PortalSessionAudit.Find(PortalSessionAuditFilter).Sort(sortDefinition)
                    .Skip(Pager.Skip(filter.CurrentPage, filter.PageSize)).Limit(filter.PageSize).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
            else
            {
                return new PaginatedResult<PortalSessionAudit>
                {
                    Records = await _PortalSessionAudit.Find(PortalSessionAuditFilter).Sort(sortDefinition).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
        }


        public Task Add(PortalSessionAudit PortalSessionAudit)
        {
            return _PortalSessionAudit.InsertOneAsync(PortalSessionAudit);
        }

        public Task Update(PortalSessionAudit PortalSessionAudit)
        {
            return _PortalSessionAudit.ReplaceOneAsync(sub => sub.Id == PortalSessionAudit.Id, PortalSessionAudit);
        }

        public Task Remove(Guid id)
        {
            return _PortalSessionAudit.DeleteOneAsync(sub => sub.Id == id.ToString());
        }

        public Task RemoveRange(DateTime date)
        {
            FilterDefinition<PortalSessionAudit> PortalSessionAuditFilter = Builders<PortalSessionAudit>.Filter.Empty;
            var dateTimeFieldDefinition = new ExpressionFieldDefinition<PortalSessionAudit, DateTime>(x => x.DateTime);
            var dateFrom = new DateTime();
            FilterDefinition<PortalSessionAudit> dateFromFilter = null;
            if (!string.IsNullOrWhiteSpace(date.ToString()))
            {
                date = date.ToLocalTime();
                var convertedDateTime = new DateTime(date.Year, date.Month, date.Day,
                    date.Hour, date.Minute, date.Second);
                dateFrom = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); ;
                dateFromFilter = Builders<PortalSessionAudit>.Filter.Lte<DateTime>(dateTimeFieldDefinition, dateFrom);
                PortalSessionAuditFilter = PortalSessionAuditFilter & Builders<PortalSessionAudit>.Filter.And(dateFromFilter);
            }
            return _PortalSessionAudit.DeleteManyAsync(PortalSessionAuditFilter);
        }

        public void Dispose()
        {
            //Db.Dispose();
        }
    }
}
