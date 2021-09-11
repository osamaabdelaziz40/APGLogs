using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APGLogs.Application.ViewModels
{
    public class SMSLogViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("ToMobile")]
        public string ToMobile { get; set; }

        [DisplayName("SMS")]
        public string SMS { get; set; }

        [Required(ErrorMessage = "The DateTime is Required")]
        [DisplayName("DateTime")]
        public DateTime DateTime { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("API")]
        public string API { get; set; }

        [DisplayName("BankId")]
        public Guid BankId { get; set; }
    }
}
