using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace WeddingPlanner
{
    public class Guest
    {
        [Key]
        public int GuestId {get;set;}

        [Required]
        [Display(Name = "First Name: ")]
        public string FirstName {get;set;}

        [Required]
        [Display(Name = "Last Name: ")]
        public string LastName {get;set;}


        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}