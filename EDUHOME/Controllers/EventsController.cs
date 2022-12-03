using EDUHOME.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EDUHOME.Controllers
{
    public class EventsController : Controller
    {
        private readonly AppDbContext _dbContext;

        public EventsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _dbContext.Events
                .Where(ev=>!ev.IsDeleted)               
                .ToListAsync(); 

            return View(events);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)  return NotFound();  

            var existEvent = await _dbContext.Events
                .Where(ev=>ev.Id == id)
                .Include(ev=>ev.EventTeachers)
                .ThenInclude(ev=>ev.Teacher)
                .FirstOrDefaultAsync();  

            if(existEvent == null) return NotFound();

            return View(existEvent);
        }

    }
}
