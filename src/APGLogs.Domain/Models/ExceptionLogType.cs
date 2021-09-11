using APGLogs.DomainHelper.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGLogs.Domain.Models
{
    public class ExceptionLogType
    {
        [BsonId]
        public string Id { get; set; }
        [Export]
        public string Name { get; set; }
        [Export]
        public string Description { get; set; }
    }
}
