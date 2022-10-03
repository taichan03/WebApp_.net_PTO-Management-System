using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LeaveManagement.Web.Models
{
    public class LeaveTypeVM
    {
        public int Id { get; set; }
        [Display(Name = "Leave Type Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Default Number Of Days")]
        [Required]
        public int DefaultDays { get; set; }


    }
}
