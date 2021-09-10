using APGLogs.DomainHelper.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace APGLogs.Domain.Models
{
    public class CommunicationLog
    {
        [BsonId]
        [Export]
        public string Id { get; set; }
        [Export]
        public string InternalRequest { get; set; }
        [Export]
        public string InternalResponse { get; set; }
        [Export]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime RequestDatetime { get; set; }
        [Export]
        public string ServiceName { get; set; }
    }
}
