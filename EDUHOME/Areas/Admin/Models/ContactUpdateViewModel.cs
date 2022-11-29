using System.ComponentModel.DataAnnotations;

namespace EDUHOME.Areas.Admin.Models
{
    public class ContactUpdateViewModel
    {
        public int Id { get; set; }
        public string Adress { get; set; }
        public string Number1 { get; set; }
        public string? Number2 { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string Website { get; set; }
    }
}
