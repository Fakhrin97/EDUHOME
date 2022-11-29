namespace EDUHOME.Areas.Admin.Models
{
    public class SliderUpdateViewModel
    {
        public string Mainheader { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }          
        public string? ImageUrl {  get; set; }   
        public IFormFile? Image {  get; set; }   
    }
}
