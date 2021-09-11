using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APGLogs.Application.ViewModels
{
    public class ShadowBalanceAuditViewModel
    {
        [Key]
        public Guid Id { get; set; }


        [Required(ErrorMessage = "The CreationDate is Required")]
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get; set; }


        [DisplayName("AuditMessage")]
        public string AuditMessage { get; set; }

        [DisplayName("ShadowBalanceId")]
        public Guid ShadowBalanceId { get; set; }

    }
}