using APGLogs.DomainHelper.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace APGLogs.Domain.Models
{
    public class CommunicationLogType
    {
        [BsonId]
        public string Id { get; set; }
        [Export]
        public string Name { get; set; }
        [Export]
        public string Description { get; set; }
    }
}
