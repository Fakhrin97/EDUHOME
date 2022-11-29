namespace EDUHOME.Areas.Admin.Models
{
    public class SliderCreateViewModel
    {
        public string Mainheader { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }    
        public IFormFile Image {  get; set; }   
    }
}
