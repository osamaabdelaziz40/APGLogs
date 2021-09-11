using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APGLogs.Application.ViewModels
{
    public class EmailLogViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("ToEmail")]
        public string ToEmail { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [Required(ErrorMessage = "The DateTime is Required")]
        [DisplayName("EmailDateTime")]
        public DateTime EmailDateTime { get; set; }

    }
}
