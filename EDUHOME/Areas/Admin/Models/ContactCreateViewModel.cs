namespace EDUHOME.Areas.Admin.Models
{
    public class ContactCreateViewModel
    {
        public string Message {  get; set; }    
        public string Address { get; set; }   
        public IFormFile AddressIcon { get; set; }
        public string Number { get; set; }
        public IFormFile TelIcon { get; set; }
        public string Website { get; set; }
        public IFormFile WebsiteIcon { get; set; } 
    }
}
