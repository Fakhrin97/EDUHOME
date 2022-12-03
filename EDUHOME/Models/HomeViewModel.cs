using EDUHOME.DAL.Entities;

namespace EDUHOME.Models
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; } = new List<Slider>();
        public List<BlogViewModel> Blogs { get; set; } = new List<BlogViewModel>();
        public List<CourseViewModel> Courses { get; set; } =new List<CourseViewModel>();
        public List < EventViewModel> Events { get; set; }  = new List<EventViewModel> ();
        
    }
}
