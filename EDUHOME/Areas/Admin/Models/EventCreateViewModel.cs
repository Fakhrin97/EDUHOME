using Microsoft.AspNetCore.Mvc.Rendering;

namespace EDUHOME.Areas.Admin.Models
{
    public class EventCreateViewModel
    {          
        public string Title { get; set; }
        public string Content { get; set; }
        public string Reply { get; set; }
        public string Venue { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public IFormFile Image { get; set; }
        public List<SelectListItem>? Teachers { get; set; }
        public List<int> TeachersId { get; set; }
    }
}
