using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APGLogs.Application.ViewModels
{
    public class CommunicationLogViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("InternalRequest")]
        public string InternalRequest { get; set; }

        [DisplayName("InternalResponse")]
        public string InternalResponse { get; set; }

        [DisplayName("RequestDatetime")]
        [Required(ErrorMessage = "The RequestDateTime is Required")]
        public DateTime RequestDatetime { get; set; }

        [DisplayName("ServiceName")]
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "The InternalRequestTime is Required")]
        [DisplayName("InternalRequestTime")]
        public DateTime InternalRequestTime { get; set; }

        [DisplayName("ExternalRequest")]
        public string ExternalRequest { get; set; }

        [Required(ErrorMessage = "The ExternalRequestTime is Required")]
        [DisplayName("ExternalRequestTime")]
        public DateTime ExternalRequestTime { get; set; }

        [DisplayName("ExternalResponse")]
        public string ExternalResponse { get; set; }

        [Required(ErrorMessage = "The ExternalResponseTime is Required")]
        [DisplayName("ExternalResponseTime")]
        public DateTime ExternalResponseTime { get; set; }

        [Required(ErrorMessage = "The InternalResponseTime is Required")]
        [DisplayName("InternalResponseTime")]
        public DateTime InternalResponseTime { get; set; }

        [DisplayName("MessageTypeId")]
        public string MessageTypeId { get; set; }

        [DisplayName("MessageFormatId")]
        public int? MessageFormatId { get; set; }


        [DisplayName("MerchantRefId")]
        public Guid MerchantRefId { get; set; }

        [DisplayName("TerminalNodeId")]
        public Guid TerminalNodeId { get; set; }

        [DisplayName("TransactionLogId")]
        public Guid TransactionLogId { get; set; }
    }
}
