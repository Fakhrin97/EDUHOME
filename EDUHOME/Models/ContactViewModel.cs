using EDUHOME.DAL.Entities;

namespace EDUHOME.Models
{
    public class ContactViewModel
    {
        public Contact Contact { get; set; } = new();
        public ContactMessageViewModel ContactMessage { get; set; }
    }
}
