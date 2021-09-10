using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APGLogs.Application.ViewModels
{
    public class ExceptionLogViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The Message is Required")]
        [DisplayName("Message")]
        public string Message { get; set; }

        [Required(ErrorMessage = "The Source is Required")]
        [DisplayName("Source")]
        public string Source { get; set; }

        [Required(ErrorMessage = "The StackTrace is Required")]
        [DisplayName("StackTrace")]
        public string StackTrace { get; set; }

        [Required(ErrorMessage = "The InnerException is Required")]
        [DisplayName("InnerException")]
        public string InnerException { get; set; }

        [DisplayName("Data")]
        public string Data { get; set; }

        [Required(ErrorMessage = "The DateTime is Required")]
        [DisplayName("DateTime")]
        public DateTime DateTime { get; set; }

        [DisplayName("ExceptionType")]
        public string ExceptionType { get; set; }

        [DisplayName("CommunicationLogId")]
        public string CommunicationLogId { get; set; }
    }
}
