using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APGLogs.Application.ViewModels
{
    public class PortalSessionAuditActionViewModel
    {
        [Key]
        public Guid Id { get; set; }


        [Required(ErrorMessage = "The ActionDate is Required")]
        [DisplayName("ActionDate")]
        public DateTime ActionDate { get; set; }


        [DisplayName("ActionName")]
        public string ActionName { get; set; }

        [DisplayName("ActionPath")]
        public string ActionPath { get; set; }

    }
}