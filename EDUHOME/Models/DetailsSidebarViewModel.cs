using EDUHOME.DAL.Entities;

namespace EDUHOME.Models
{
    public class DetailsSidebarViewModel
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Blog> Blogs { get; set; } = new List<Blog>();   

    }
}
