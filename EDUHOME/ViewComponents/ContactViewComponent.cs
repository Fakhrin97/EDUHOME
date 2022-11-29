using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.ViewComponents
{
    public class ContactViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public ContactViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var contact = await _dbContext.Contacts
                .FirstOrDefaultAsync();

            return View(contact);
        }
       
    }
}
