using APGLogs.DomainHelper.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace APGLogs.Domain.Models
{
    public class PortalSessionAudit
    {
        [BsonId]
        [Export]
        public string Id { get; set; }
        [Export]
        public string SessionID { get; set; }


        [Export]
        public string IPAddress { get; set; }

        [Export]
        public string UserName { get; set; }

        [Export]
        public Guid UserId { get; set; }

    }
}
