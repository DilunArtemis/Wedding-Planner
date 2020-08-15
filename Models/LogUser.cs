using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class LogUser
    {
        [Required(ErrorMessage = "Enter your email")]
        [EmailAddress(ErrorMessage = "Email address is needed to login")]
        [Display(Name="Email: ")]
        public string LogEmail { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        [Display(Name="Password: ")]
        [DataType(DataType.Password)]
        public string LogPass { get; set; }
    }
}