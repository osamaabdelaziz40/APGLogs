using APGLogs.DomainHelper.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace APGLogs.Domain.Models
{
    public class ShadowBalanceAudit
    {
        [BsonId]
        [Export]
        public string Id { get; set; }

        [Export]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreationDate { get; set; }


        [Export]
        public string AuditMessage { get; set; }

        [Export]
        public Guid ShadowBalanceId { get; set; }


    }
}
