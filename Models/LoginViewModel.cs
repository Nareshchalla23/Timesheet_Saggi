using System.ComponentModel.DataAnnotations;

namespace TImesheet_demo2.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Employee ID")]
        public string EmployeeID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
