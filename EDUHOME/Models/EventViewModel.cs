namespace EDUHOME.Models
{
    public class EventViewModel
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Venue { get; set; }

    }
}
