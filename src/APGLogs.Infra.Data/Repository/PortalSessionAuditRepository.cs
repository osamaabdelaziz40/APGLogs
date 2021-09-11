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

        public void Dispose()
        {
            //Db.Dispose();
        }
    }
}
