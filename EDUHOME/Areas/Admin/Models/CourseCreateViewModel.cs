using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EDUHOME.Areas.Admin.Models
{
    public class CourseCreateViewModel
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string About { get; set; }
        public string Apply { get; set; }
        public string Certification { get; set; }
        public string Reply { get; set; }
        public DateTime GeginningTime { get; set; }
        public int Duration { get; set; }
        public int ClassDuration { get; set; }
        public string Level { get; set; }
        public string Language { get; set; }
        public int StudentsCount { get; set; }
        public string Assesments { get; set; }
        public int Fee { get; set; }
        public IFormFile Image { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public int CategoryId { get; set; }
    }
}
