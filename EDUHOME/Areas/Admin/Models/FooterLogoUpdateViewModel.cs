namespace EDUHOME.Areas.Admin.Models
{
    public class FooterLogoUpdateViewModel
    {
        public string LogoTitle { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
