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
    public class ExceptionLogTypeRepository : IExceptionLogTypeRepository
    {
        private readonly IMongoCollection<ExceptionLogType> _exceptionLogType;

        public ExceptionLogTypeRepository(IAPGLogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _exceptionLogType = database.GetCollection<ExceptionLogType>(settings.ExceptionLogTypeCollectionName);
        }

        public async Task<ExceptionLogType> GetById(Guid id)
        {
            //return await DbSet.FindAsync(id);
            return await _exceptionLogType.Find<ExceptionLogType>(x => x.Id == id.ToString()).FirstOrDefaultAsync();
        }
       
        public async Task<IEnumerable<ExceptionLogType>> GetAll()
        {
            List<ExceptionLogType> exceptionLogsTypes;
            exceptionLogsTypes = await _exceptionLogType.Find(x => true).ToListAsync();
            return exceptionLogsTypes;
        }

       
        public Task Add(ExceptionLogType exceptionLogType)
        {
            return _exceptionLogType.InsertOneAsync(exceptionLogType);
        }

        public Task Update(ExceptionLogType exceptionLogType)
        {
            return _exceptionLogType.ReplaceOneAsync(sub => sub.Id == exceptionLogType.Id, exceptionLogType);
        }

        public Task Remove(Guid id)
        {
            return _exceptionLogType.DeleteOneAsync(sub => sub.Id == id.ToString());
        }

        public void Dispose()
        {
            //Db.Dispose();
        }
    }
}
