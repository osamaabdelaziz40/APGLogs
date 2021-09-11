using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGLogs.Application.ViewModels
{
    public class ExceptionLogTypeViewModel
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
