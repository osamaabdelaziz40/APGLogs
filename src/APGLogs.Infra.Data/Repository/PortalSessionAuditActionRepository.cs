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
    public class PortalSessionAuditActionRepository : IPortalSessionAuditActionRepository
    {
        private readonly IMongoCollection<PortalSessionAuditAction> _PortalSessionAuditAction;

        public PortalSessionAuditActionRepository(IAPGLogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _PortalSessionAuditAction = database.GetCollection<PortalSessionAuditAction>(settings.PortalSessionAuditActionCollectionName);
        }


        public async Task<PortalSessionAuditAction> GetById(Guid id)
        {
            //return await DbSet.FindAsync(id);
            return await _PortalSessionAuditAction.Find<PortalSessionAuditAction>(x => x.Id == id.ToString()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PortalSessionAuditAction>> GetAll()
        {
            List<PortalSessionAuditAction> PortalSessionAuditActions;
            PortalSessionAuditActions = await _PortalSessionAuditAction.Find(x => true).ToListAsync();
            return PortalSessionAuditActions;
        }
        public async Task<PaginatedResult<PortalSessionAuditAction>> GetPaginatedResultAsync(PortalSessionAuditActionFilter filter)
        {
            FilterDefinition<PortalSessionAuditAction> PortalSessionAuditActionFilter = Builders<PortalSessionAuditAction>.Filter.Empty;
            var dateTimeFieldDefinition = new ExpressionFieldDefinition<PortalSessionAuditAction, DateTime>(x => x.ActionDate);

            var dateFrom = new DateTime();
            FilterDefinition<PortalSessionAuditAction> dateFromFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateFrom.ToString()) && filter.DateFrom > DateTime.MinValue)
            {
                filter.DateFrom = filter.DateFrom.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateFrom.Year, filter.DateFrom.Month, filter.DateFrom.Day,
                    filter.DateFrom.Hour, filter.DateFrom.Minute, filter.DateFrom.Second);
                dateFrom = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); ;
                dateFromFilter = Builders<PortalSessionAuditAction>.Filter.Gte<DateTime>(dateTimeFieldDefinition, dateFrom);
                PortalSessionAuditActionFilter = PortalSessionAuditActionFilter & Builders<PortalSessionAuditAction>.Filter.And(dateFromFilter);
            }

            var dateTo = new DateTime();
            FilterDefinition<PortalSessionAuditAction> dateToFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateTo.ToString()) && filter.DateTo > DateTime.MinValue)
            {
                filter.DateTo = filter.DateTo.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateTo.Year, filter.DateTo.Month, filter.DateTo.Day,
                    filter.DateTo.Hour, filter.DateTo.Minute, filter.DateTo.Second);
                dateTo = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                dateToFilter = Builders<PortalSessionAuditAction>.Filter.Lte<DateTime>(dateTimeFieldDefinition, dateTo);
                PortalSessionAuditActionFilter = PortalSessionAuditActionFilter & Builders<PortalSessionAuditAction>.Filter.And(dateToFilter);
            }

            var sortDefinition = Builders<PortalSessionAuditAction>.Sort.Descending(a => a.ActionDate);

            ProjectionDefinition<PortalSessionAuditAction> project = Builders<PortalSessionAuditAction>
                    .Projection.Include(x => x.Id);
            project.Include(x => x.ActionName);
            project.Include(x => x.ActionPath);

            var queryCount = _PortalSessionAuditAction.Find(PortalSessionAuditActionFilter).Project(project).CountDocumentsAsync().Result;

            if (!filter.IsExport)
            {
                return new PaginatedResult<PortalSessionAuditAction>
                {
                    Records = await _PortalSessionAuditAction.Find(PortalSessionAuditActionFilter).Sort(sortDefinition)
                    .Skip(Pager.Skip(filter.CurrentPage, filter.PageSize)).Limit(filter.PageSize).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
            else
            {
                return new PaginatedResult<PortalSessionAuditAction>
                {
                    Records = await _PortalSessionAuditAction.Find(PortalSessionAuditActionFilter).Sort(sortDefinition).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
        }

        public Task Add(PortalSessionAuditAction PortalSessionAuditAction)
        {
            return _PortalSessionAuditAction.InsertOneAsync(PortalSessionAuditAction);
        }

        public Task Update(PortalSessionAuditAction PortalSessionAuditAction)
        {
            return _PortalSessionAuditAction.ReplaceOneAsync(sub => sub.Id == PortalSessionAuditAction.Id, PortalSessionAuditAction);
        }

        public Task Remove(Guid id)
        {
            return _PortalSessionAuditAction.DeleteOneAsync(sub => sub.Id == id.ToString());
        }
        public Task RemoveRange(DateTime date)
        {
            FilterDefinition<PortalSessionAuditAction> PortalSessionAuditActionFilter = Builders<PortalSessionAuditAction>.Filter.Empty;
            var dateTimeFieldDefinition = new ExpressionFieldDefinition<PortalSessionAuditAction, DateTime>(x => x.ActionDate);
            var dateFrom = new DateTime();
            FilterDefinition<PortalSessionAuditAction> dateFromFilter = null;
            if (!string.IsNullOrWhiteSpace(date.ToString()))
            {
                date = date.ToLocalTime();
                var convertedDateTime = new DateTime(date.Year, date.Month, date.Day,
                    date.Hour, date.Minute, date.Second);
                dateFrom = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); ;
                dateFromFilter = Builders<PortalSessionAuditAction>.Filter.Lte<DateTime>(dateTimeFieldDefinition, dateFrom);
                PortalSessionAuditActionFilter = PortalSessionAuditActionFilter & Builders<PortalSessionAuditAction>.Filter.And(dateFromFilter);
            }
            return _PortalSessionAuditAction.DeleteManyAsync(PortalSessionAuditActionFilter);
        }

        public void Dispose()
        {
            //Db.Dispose();
        }
    }
}
