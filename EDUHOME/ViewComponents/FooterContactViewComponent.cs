using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.ViewComponents
{
    public class FooterContactViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public FooterContactViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var contact = await _dbContext.FooterContacts
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync();

            return View(contact);
        }
       
    }
}
