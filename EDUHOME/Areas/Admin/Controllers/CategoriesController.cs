using EDUHOME.Areas.Admin.Models;
using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.Areas.Admin.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public CategoriesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.Categories
                .ToListAsync();

            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var existCategory = await _dbContext.Categories
                .Where(category => category.Name == model.Name)
                .FirstOrDefaultAsync();

            if (existCategory != null)
            {
                ModelState.AddModelError("name", "Bu Adda artiq bazada Informasiya var");
                return View();
            }

            await _dbContext.Categories.AddAsync(new Category
            {
                Name = model.Name,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var category = await _dbContext.Categories
                .Where(info => info.Id == id)
                .FirstOrDefaultAsync();

            var model = new CategoryUpdateViewModel
            {
                Id = id,
                Name = category.Name,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, CategoryUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var existCategory = await _dbContext.Categories
                 .Where(category => category.Id == id)
                 .FirstOrDefaultAsync();

            var result = await _dbContext.Categories
                .Where(category => category.Name == model.Name)
                .FirstOrDefaultAsync();

            if (existCategory.Name != model.Name)
            {
                if (result != null)
                {
                    ModelState.AddModelError("name", "Bu adda artiq informasiya movcuddur");
                    return View();
                }
            }

            existCategory.Name = model.Name;          

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existCategory = await _dbContext.Categories
                .Where(category => category.Id == id)
                .FirstOrDefaultAsync();

            if (existCategory == null) return BadRequest();

            _dbContext.Categories.Remove(existCategory);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
