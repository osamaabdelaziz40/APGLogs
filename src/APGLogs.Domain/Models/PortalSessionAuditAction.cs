﻿using APGLogs.DomainHelper.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace APGLogs.Domain.Models
{
    public class PortalSessionAuditAction
    {
        [BsonId]
        [Export]
        public string Id { get; set; }

        [Export]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ActionDate { get; set; }


        [Export]
        public string ActionName { get; set; }

        [Export]
        public string ActionPath { get; set; }


    }
}
