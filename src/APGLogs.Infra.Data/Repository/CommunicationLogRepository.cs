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
    public class CommunicationLogRepository : ICommunicationLogRepository
    {
        private readonly IMongoCollection<CommunicationLog> _communicationLog;

        public CommunicationLogRepository(IAPGLogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _communicationLog = database.GetCollection<CommunicationLog>(settings.CommunicationLogCollectionName);
        }

        public async Task<CommunicationLog> GetById(Guid id)
        {
            //return await DbSet.FindAsync(id);
            return await _communicationLog.Find<CommunicationLog>(x => x.Id == id.ToString()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CommunicationLog>> GetAll()
        {
            List<CommunicationLog> communicationLogs;
            communicationLogs = await _communicationLog.Find(x => true).ToListAsync();
            return communicationLogs;
        }

        public async Task<PaginatedResult<CommunicationLog>> GetPaginatedResultAsync(CommunicationLogFilter filter)
        {
            FilterDefinition<CommunicationLog> communicationLogFilter = Builders<CommunicationLog>.Filter.Empty;
            var serviceNameFieldDefinition = new ExpressionFieldDefinition<CommunicationLog, string>(x => x.ServiceName);
            var dateTimeFieldDefinition = new ExpressionFieldDefinition<CommunicationLog, DateTime>(x => x.RequestDatetime);

            FilterDefinition<CommunicationLog> serviceNameFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.ServiceName))
            {
                serviceNameFilter = Builders<CommunicationLog>.Filter.Eq(serviceNameFieldDefinition, filter.ServiceName);
                communicationLogFilter = communicationLogFilter & Builders<CommunicationLog>.Filter.And(serviceNameFilter);
            }

            var dateFrom = new DateTime();
            FilterDefinition<CommunicationLog> dateFromFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateFrom.ToString()) && filter.DateFrom > DateTime.MinValue)
            {
                filter.DateFrom = filter.DateFrom.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateFrom.Year, filter.DateFrom.Month, filter.DateFrom.Day,
                    filter.DateFrom.Hour, filter.DateFrom.Minute, filter.DateFrom.Second);
                dateFrom = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); ;
                dateFromFilter = Builders<CommunicationLog>.Filter.Gte<DateTime>(dateTimeFieldDefinition, dateFrom);
                communicationLogFilter = communicationLogFilter & Builders<CommunicationLog>.Filter.And(dateFromFilter);
            }

            var dateTo = new DateTime();
            FilterDefinition<CommunicationLog> dateToFilter = null;
            if (!string.IsNullOrWhiteSpace(filter.DateTo.ToString()) && filter.DateTo > DateTime.MinValue)
            {
                filter.DateTo = filter.DateTo.ToLocalTime();
                var convertedDateTime = new DateTime(filter.DateTo.Year, filter.DateTo.Month, filter.DateTo.Day,
                    filter.DateTo.Hour, filter.DateTo.Minute, filter.DateTo.Second);
                dateTo = DateTime.ParseExact(convertedDateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                dateToFilter = Builders<CommunicationLog>.Filter.Lte<DateTime>(dateTimeFieldDefinition, dateTo);
                communicationLogFilter = communicationLogFilter & Builders<CommunicationLog>.Filter.And(dateToFilter);
            }

            var sortDefinition = Builders<CommunicationLog>.Sort.Descending(a => a.RequestDatetime);

            ProjectionDefinition<CommunicationLog> project = Builders<CommunicationLog>
                    .Projection.Include(x => x.Id);
            project.Include(x => x.InternalRequest);
            project.Include(x => x.InternalResponse);
            project.Include(x => x.ServiceName);

            var queryCount = _communicationLog.Find(communicationLogFilter).Project(project).CountDocumentsAsync().Result;

            if (!filter.IsExport)
            {
                return new PaginatedResult<CommunicationLog>
                {
                    Records = await _communicationLog.Find(communicationLogFilter).Sort(sortDefinition)
                    .Skip(Pager.Skip(filter.CurrentPage, filter.PageSize)).Limit(filter.PageSize).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
            else
            {
                return new PaginatedResult<CommunicationLog>
                {
                    Records = await _communicationLog.Find(communicationLogFilter).Sort(sortDefinition).ToListAsync(),
                    Total = Convert.ToInt32(queryCount),
                    HasNext = Pager.HasMoreItems(Convert.ToInt32(queryCount), filter.CurrentPage, filter.PageSize)
                };
            }
        }


        public Task Add(CommunicationLog communicationLog)
        {
            return _communicationLog.InsertOneAsync(communicationLog);
        }

        public Task Update(CommunicationLog communicationLog)
        {
            return _communicationLog.ReplaceOneAsync(sub => sub.Id == communicationLog.Id, communicationLog);
        }

        public Task Remove(Guid id)
        {
            return _communicationLog.DeleteOneAsync(sub => sub.Id == id.ToString());
        }

        public void Dispose()
        {
            //Db.Dispose();
        }
    }
}
