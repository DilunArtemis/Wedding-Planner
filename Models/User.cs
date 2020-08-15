using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace WeddingPlanner.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}

        [Required (ErrorMessage = "First name is required")]
        [MinLength(2, ErrorMessage = "First name should be at least 2 characters.")]
        [Display(Name = "First Name: ")]
        public string FirstName {get;set;}

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name: ")]
        public string LastName {get;set;}

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email: ")]
        public string Email {get;set;}

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters!")]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$", ErrorMessage="Password must contain at least 1 letter and 1 number")]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        [NotMapped]
        [Display(Name = "Confirm Password: ")]
        [Compare("Password",ErrorMessage = "Passwords must match!")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}

        public List<Wedding> WeddingsPlanned {get;set;}
        public List<RSVP> WeddingsAttending {get;set;}
    
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}