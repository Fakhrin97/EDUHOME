using System.Data;

namespace EDUHOME.DAL.Entities
{
    public class Blog : Entity
    {
        public string ImageUrl {  get; set; }  
        public string Title { get; set; }   
        public string Content { get; set; } 
        public string Reply { get; set; }          
        public Nullable<DateTime> CreatedAt { get; set; } 
    }
}
