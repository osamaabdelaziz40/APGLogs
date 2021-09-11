using APGLogs.DomainHelper.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace APGLogs.Domain.Models
{
    public class EmailLog
    {
        [BsonId]
        [Export]
        public string Id { get; set; }
        [Export]
        public string ToEmail { get; set; }

        [Export]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime EmailDateTime { get; set; }

        [Export]
        public string Status { get; set; }


    }
}
