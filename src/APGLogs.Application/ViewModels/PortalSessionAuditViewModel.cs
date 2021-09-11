using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APGLogs.Application.ViewModels
{
    public class PortalSessionAuditViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("SessionID")]
        public string SessionID { get; set; }

        [DisplayName("IPAddress")]
        public string IPAddress { get; set; }


        [DisplayName("UserName")]
        public string UserName { get; set; }


        [DisplayName("UserId")]
        public Guid UserId { get; set; }
    }
}

