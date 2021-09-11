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
    public class CommunicationLogTypeRepository : ICommunicationLogTypeRepository
    {
        private readonly IMongoCollection<CommunicationLogType> _communicationLogType;

        public CommunicationLogTypeRepository(IAPGLogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _communicationLogType = database.GetCollection<CommunicationLogType>(settings.CommunicationLogTypeCollectionName);
        }

        public async Task<CommunicationLogType> GetById(Guid id)
        {
            //return await DbSet.FindAsync(id);
            return await _communicationLogType.Find<CommunicationLogType>(x => x.Id == id.ToString()).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CommunicationLogType>> GetAll()
        {
            List<CommunicationLogType> communicationLogsTypes;
            communicationLogsTypes = await _communicationLogType.Find(x => true).ToListAsync();
            return communicationLogsTypes;
        }


        public Task Add(CommunicationLogType communicationLogType)
        {
            return _communicationLogType.InsertOneAsync(communicationLogType);
        }

        public Task Update(CommunicationLogType communicationLogType)
        {
            return _communicationLogType.ReplaceOneAsync(sub => sub.Id == communicationLogType.Id, communicationLogType);
        }

        public Task Remove(Guid id)
        {
            return _communicationLogType.DeleteOneAsync(sub => sub.Id == id.ToString());
        }

        public void Dispose()
        {
            //Db.Dispose();
        }
    }
}
