using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId {get;set;}

        [Required(ErrorMessage = "Who is getting married?")]
        [Display(Name = "Getting Married Person A: ")]
        public string WedderOne {get;set;}
        
        [Required(ErrorMessage = "Who is getting married to the first person??")]
        [Display(Name = "Getting Married Person B: ")]
        public string WedderTwo {get;set;}

        [Required(ErrorMessage = "What date is the wedding?")]
        [Display(Name = "Date: ")]
        public DateTime? Date{get;set;}

        [Required(ErrorMessage = "Where is the wedding?")]
        [Display (Name = "Address of event:")]
        public string Address {get;set;}
        public int UserId{get;set;}
        public User Planner {get;set;}

        public List<RSVP> GuestsAttending {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}