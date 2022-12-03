using Allup.Data;
using EDUHOME.Areas.Admin.Data;
using EDUHOME.Areas.Admin.Models;
using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.Areas.Admin.Controllers
{
    public class HeaderLogoController : BaseController
    {
        private readonly AppDbContext _dbContext;
        public HeaderLogoController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var headerLogo = await _dbContext.HeaderLogos
                .ToListAsync();

            return View(headerLogo);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HeaderLogoCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("", "Shekil Secmelisiz");
                return View();
            }

            if (!model.Image.IsAllowedSize(10))
            {
                ModelState.AddModelError("", "Shekilin olcusu 10 mbdan az omalidi");
                return View();
            }

            var unicalName = await model.Image.GenerateFile(Constants.HeaderLogoPath);

            await _dbContext.HeaderLogos.AddAsync(new HeaderLogo
            {
                ImageUrl = unicalName,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            var headerLogo = await _dbContext.HeaderLogos
                .Where(headerLogo => headerLogo.Id == id)
                .FirstOrDefaultAsync();

            if (headerLogo == null) return BadRequest();

            var model = new HeaderLogoUpdateViewModel
            {
                ImageUrl = headerLogo.ImageUrl,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, HeaderLogoUpdateViewModel model)
        {
            if (id == null) return BadRequest();

            var existHeaderLogo = await _dbContext.HeaderLogos
                .Where(headerLogo => headerLogo.Id == id)
                .FirstOrDefaultAsync();

            var viewModel = new HeaderLogoUpdateViewModel
            {
                ImageUrl = existHeaderLogo.ImageUrl,
            };

            if (!ModelState.IsValid) return View(viewModel);


            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Shekil Secmelisiz");
                return View(viewModel);
            }

            if (!model.Image.IsAllowedSize(10))
            {
                ModelState.AddModelError("Image", "Shekilin olcusu 10 mbdan az omalidi");
                return View(viewModel);
            }

            var path = Path.Combine(Constants.RootPath, "assets", "images", "headerlogo", existHeaderLogo.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            var unicalName = await model.Image.GenerateFile(Constants.HeaderLogoPath);

            existHeaderLogo.ImageUrl = unicalName;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var existHeaderLogo = await _dbContext.HeaderLogos
                .Where(headerLogo => headerLogo.Id == id)
                .FirstOrDefaultAsync();

            if (existHeaderLogo == null) return BadRequest();

            _dbContext.HeaderLogos.Remove(existHeaderLogo);

            var path = Path.Combine(Constants.RootPath, "assets", "images", "headerlogo", existHeaderLogo.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
