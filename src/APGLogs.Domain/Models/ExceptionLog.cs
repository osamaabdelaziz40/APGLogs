using MongoDB.Bson.Serialization.Attributes;
using NetDevPack.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using APGLogs.DomainHelper.Attributes;

namespace APGLogs.Domain.Models
{
    public class ExceptionLog
    {
        //public ExceptionLog(string message, string source, string stackTrace, string innerException,
        //    string data, DateTime dateTime, string exceptionType, long? communicationLogId)
        //{
        //    Message = message;
        //    Source = source;
        //    StackTrace = stackTrace;
        //    InnerException = innerException;
        //    Data = data;
        //    //DataTime = dateTime;
        //    ExceptionType = exceptionType;
        //    CommunicationLogId = communicationLogId;
        //}

        [BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Export]
        public string Message { get; set; }
        [Export]
        public string Source { get; set; }
        [Export]
        public string StackTrace { get; set; }
        [Export]
        public string InnerException { get; set; }
        [Export]
        public string Data { get; set; }
        [Export]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DateTime
        {
            get;
            set;
        }
        [Export]
        public string ExceptionType { get; set; }
        [Export]
        public string CommunicationLogId { get; set; }
    }
}
