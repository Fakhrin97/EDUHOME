using EDUHOME.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.ViewComponents
{
    public class HeaderLogoViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public HeaderLogoViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerLogo = await _dbContext.HeaderLogos
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync();

            return View(headerLogo);
        }
    }
}
