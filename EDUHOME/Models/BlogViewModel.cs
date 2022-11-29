namespace EDUHOME.Models
{
    public class BlogViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Reply { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
    }
}
