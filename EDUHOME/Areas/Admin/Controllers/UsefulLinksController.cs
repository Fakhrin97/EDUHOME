using EDUHOME.Areas.Admin.Models;
using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.Areas.Admin.Controllers
{
    public class UsefulLinksController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public UsefulLinksController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task<IActionResult> Index()
        {
            var links = await _dbContext.UsefulLinks
                .ToListAsync();

            return View(links);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsefulLinkCreateViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var existLink = await _dbContext.UsefulLinks
                .Where(info => info.Name == model.Name)
                .FirstOrDefaultAsync();

            if (existLink != null)
            {
                ModelState.AddModelError("name", "Bu Adda artiq bazada Informasiya var");
                return View();
            }

            await _dbContext.UsefulLinks.AddAsync(new UsefulLink
            {
                Name = model.Name,
                Url = model.Url
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var link = await _dbContext.UsefulLinks
                .Where(link => link.Id == id)
                .FirstOrDefaultAsync();

            var model = new UsefulLinkUpdateViewModel
            {
                Id = id,
                Name = link.Name,
                Url = link.Url
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UsefulLinkUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var existLink = await _dbContext.UsefulLinks
                 .Where(link => link.Id == id)
                 .FirstOrDefaultAsync();

            var result = await _dbContext.UsefulLinks
                .Where(link => link.Name == model.Name)
                .FirstOrDefaultAsync();

            if (result != null)
            {
                ModelState.AddModelError("name", "Bu adda artiq informasiya movcuddur");
                return View();
            }

            existLink.Name = model.Name;
            existLink.Url = model.Url;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existLink = await _dbContext.UsefulLinks
                .Where(link => link.Id == id)
                .FirstOrDefaultAsync();

            if (existLink == null) return BadRequest();

            _dbContext.UsefulLinks.Remove(existLink);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
