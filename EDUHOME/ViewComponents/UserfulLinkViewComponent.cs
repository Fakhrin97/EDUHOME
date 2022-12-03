using EDUHOME.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.ViewCompanents
{
    public class UserfulLinkViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public UserfulLinkViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var links = await _dbContext.UsefulLinks
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            return View(links);
        }
    }
}
