namespace EDUHOME.DAL.Entities
{
    public class EventTeacher : Entity
    {
        public int TeacherId { get; set; }  
        public Teacher Teacher { get; set; }    
        public int EventId {  get; set; }   
        public Event Event { get; set; }    
    }
}
