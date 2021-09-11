using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APGLogs.Application.ViewModels
{
    public class CommunicationLogTypeViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The Name is Required")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Description is Required")]
        [DisplayName("Source")]
        public string Description { get; set; }
    }
}
