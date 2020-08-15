using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class DashWrapper
    {
        public User LoggedUser {get;set;}
        public List<Wedding> AllWeddings {get;set;}
    }
    
}