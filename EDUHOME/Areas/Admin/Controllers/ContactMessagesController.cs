using EDUHOME.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.Areas.Admin.Controllers
{
    public class ContactMessagesController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public ContactMessagesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _dbContext.ContactMessages
                .ToListAsync();

            return View(messages);
        }

        public async Task<IActionResult> Details(int id)
        {
            var message = await _dbContext.ContactMessages
                .Where(message => message.Id == id)
                .FirstOrDefaultAsync();

            if(message == null) return View();   

            message.IsRead = true;

            await _dbContext.SaveChangesAsync();

            return View(message);
        }
    }
}
