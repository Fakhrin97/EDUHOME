namespace EDUHOME.DAL.Entities
{
    public class Contact : Entity
    {
        public string Message { get; set; }
        public string Address { get; set; } 
        public string AddressImageUrl { get; set; }
        public string ContactNumber { get; set; }
        public string ContactNumberImageUrl { get; set; }
        public string Website { get; set; }
        public string WebsiteImageUrl { get; set; }
    }
}
