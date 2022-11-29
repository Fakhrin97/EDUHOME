using System.ComponentModel.DataAnnotations;

namespace EDUHOME.Areas.Admin.Models
{
    public class TeacherUpdateViewModel
    {
        public string Name { get; set; }
        public string Profession { get; set; }
        public string About { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        [EmailAddress]
        public string Mail { get; set; }
        public string Skype { get; set; }
        public string Number { get; set; }
        public int Language { get; set; }
        public int Teamleader { get; set; }
        public int Development { get; set; }
        public int Design { get; set; }
        public int Innovation { get; set; }
        public int Communication { get; set; }
        public string? Imageurl { get; set; }    
        public IFormFile? Image { get; set; }
    }
}
