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
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime InternalRequestTime { get; set; }

        [Export]
        public string ExternalRequest { get; set; }

        [Export]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ExternalRequestTime { get; set; }

        [Export]
        public string ExternalResponse { get; set; }

        [Export]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ExternalResponseTime { get; set; }

        [Export]
        public string InternalResponse { get; set; }

        [Export]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime InternalResponseTime { get; set; }
        [Export]
        public string MessageTypeId { get; set; }
        [Export]
        public int? MessageFormatId { get; set; }

        [Export]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime RequestDatetime { get; set; }

        [Export]
        public string ServiceName { get; set; }
        [Export]
        public Guid MerchantRefId { get; set; }
        [Export]
        public Guid TerminalNodeId { get; set; }
        [Export]
        public Guid TransactionLogId { get; set; }
    }
}
