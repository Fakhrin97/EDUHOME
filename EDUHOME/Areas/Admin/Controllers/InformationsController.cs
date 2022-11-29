using EDUHOME.Areas.Admin.Models;
using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.Areas.Admin.Controllers
{
    public class InformationsController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public InformationsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var informations = await _dbContext.Informations
                .ToListAsync();

            return View(informations);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InformationCreateViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var existInformation = await _dbContext.Informations
                .Where(info => info.Name == model.Name)
                .FirstOrDefaultAsync();

            if (existInformation != null)
            {
                ModelState.AddModelError("name", "Bu Adda artiq bazada Informasiya var");
                return View();
            }

            await _dbContext.Informations.AddAsync(new Information
            {
                Name = model.Name,
                Url = model.Url
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var info = await _dbContext.Informations
                .Where(info => info.Id == id)
                .FirstOrDefaultAsync();

            var model = new InformationUpdateViewModel
            {
                Id = id,
                Name = info.Name,
                Url = info.Url
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, InformationUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var existInformation = await _dbContext.Informations
                 .Where(info => info.Id == id)
                 .FirstOrDefaultAsync();

            var result = await _dbContext.Informations
                .Where(info => info.Name == model.Name)
                .FirstOrDefaultAsync();

            if (existInformation.Name != model.Name)
            {
                if (result != null)
                {
                    ModelState.AddModelError("name", "Bu adda artiq informasiya movcuddur");
                    return View();
                }
            }

            existInformation.Name = model.Name;
            existInformation.Url = model.Url;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existInfo = await _dbContext.Informations
                .Where(info => info.Id == id)
                .FirstOrDefaultAsync();

            if (existInfo == null) return BadRequest();

            _dbContext.Informations.Remove(existInfo);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
