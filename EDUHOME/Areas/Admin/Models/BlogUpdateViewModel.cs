namespace EDUHOME.Areas.Admin.Models
{
    public class BlogUpdateViewModel
    {
        public string? ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Reply { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
    }
}
