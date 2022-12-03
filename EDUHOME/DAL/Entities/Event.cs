namespace EDUHOME.DAL.Entities
{
    public class Event : Entity
    {
        public string ImageUrl { get; set; }    
        public string Title { get; set; }   
        public string Content { get; set; }   
        public string Reply { get; set; }   
        public string Venue { get; set; }   
        public DateTime StartTime { get; set; }   
        public DateTime EndTime { get; set; }   
        public List<EventTeacher> EventTeachers { get; set; }    
    }
}
