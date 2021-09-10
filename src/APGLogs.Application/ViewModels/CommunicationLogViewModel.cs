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
    }
}
