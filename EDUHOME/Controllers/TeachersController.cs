using EDUHOME.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.Controllers
{
    public class TeachersController : Controller
    {
        private readonly AppDbContext _dbContext;

        public TeachersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var teachers = _dbContext.Teachers.ToList();

            return View(teachers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();  

            var teacher = await _dbContext.Teachers
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(); 

            return View(teacher);
        }

    }
}
