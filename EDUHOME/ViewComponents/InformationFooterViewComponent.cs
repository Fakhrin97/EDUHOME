using EDUHOME.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.ViewCompanents
{
    public class InformationFooterViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public InformationFooterViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var infos = await _dbContext.Informations
                .ToListAsync();

            return View(infos);
        }
    }
}
