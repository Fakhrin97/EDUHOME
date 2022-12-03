namespace EDUHOME.Areas.Admin.Models
{
    public class ContactUpdateViewModel
    {
        public string Message { get; set; }
        public string Address { get; set; }
        public string? AddressImageUrl { get; set; }
        public IFormFile? AddressIcon { get; set; }
        public string Number { get; set; }
        public string? TelImageUrl { get; set; }
        public IFormFile? TelIcon { get; set; }
        public string Website { get; set; }
        public string? WebsiteImageUrl { get; set; }
        public IFormFile? WebsiteIcon { get; set; }
    }
}
