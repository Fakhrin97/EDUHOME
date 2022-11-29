using EDUHOME.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.ViewComponents
{
    public class FooterSocialMediaViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;
        public FooterSocialMediaViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerSocialMedia = await _dbContext.FooterSocialMedias
                .ToListAsync();

            return View(footerSocialMedia);
        }
    }
}
