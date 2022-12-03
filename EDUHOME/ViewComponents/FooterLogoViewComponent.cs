using EDUHOME.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.ViewComponents
{
    public class FooterLogoViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public FooterLogoViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerLogo = await _dbContext.FooterLogo
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync();

            return View(footerLogo);
        }
    }
}
