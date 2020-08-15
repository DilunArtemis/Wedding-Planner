using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class RSVP
    {
        [Key]
        public int RSVPId {get;set;}

        public int UserId {get;set;}
        public User Guest {get;set;}
        public int WeddingId {get;set;}
        public Wedding Wedding {get;set;}
        

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}