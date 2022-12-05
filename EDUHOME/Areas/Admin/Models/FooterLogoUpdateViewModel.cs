namespace EDUHOME.Areas.Admin.Models
{
    public class FooterLogoUpdateViewModel
    {
        public bool IsDeleted { get; set; }

        public string LogoTitle { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
