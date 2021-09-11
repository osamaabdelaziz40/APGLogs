using APGLogs.DomainHelper.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace APGLogs.Domain.Models
{
    public class SMSLog
    {
        [BsonId]
        [Export]
        public string Id { get; set; }
        [Export]
        public string ToMobile { get; set; }

        [Export]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DateTime { get; set; }

        [Export]
        public string SMS { get; set; }

        [Export]
        public string Status { get; set; }

        [Export]
        public string API { get; set; }
        [Export]
        public Guid BankId { get; set; }

    }
}
