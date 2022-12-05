using Allup.Data;
using EDUHOME.Areas.Admin.Data;
using EDUHOME.Areas.Admin.Models;
using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.Areas.Admin.Controllers
{
    public class FooterLogoController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public FooterLogoController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var footerLogo = await _dbContext.FooterLogo
                .ToListAsync();

            return View(footerLogo);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FooterLogoCreateViewModel model)
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

            var unicalName = await model.Image.GenerateFile(Constants.FooterLogoPath);

            await _dbContext.FooterLogo.AddAsync(new FooterLogo
            {
                LogoImgUrl = unicalName,
                LogoTitle = model.LogoTitle
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            var footerLogo = await _dbContext.FooterLogo
                .Where(footerLogo => footerLogo.Id == id)
                .FirstOrDefaultAsync();

            if (footerLogo == null) return BadRequest();

            var model = new FooterLogoUpdateViewModel
            {
                LogoTitle =footerLogo.LogoTitle,
                ImageUrl = footerLogo.LogoImgUrl,
                IsDeleted = footerLogo.IsDeleted,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, FooterLogoUpdateViewModel model)
        {
            if (id == null) return BadRequest();

            var existFooteLogo = await _dbContext.FooterLogo
                .Where(footerLogo => footerLogo.Id == id)
                .FirstOrDefaultAsync();

            var viewModel = new FooterLogoUpdateViewModel
            {
                LogoTitle= existFooteLogo.LogoTitle,
                ImageUrl = existFooteLogo.LogoImgUrl,
                IsDeleted = existFooteLogo.IsDeleted,
            };

            if (!ModelState.IsValid) return View(viewModel);

            if (model.Image != null)
            {
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

                var path = Path.Combine(Constants.RootPath, "assets", "images", "footerlogo", existFooteLogo.LogoImgUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalName = await model.Image.GenerateFile(Constants.FooterLogoPath);

                existFooteLogo.LogoImgUrl = unicalName;
            }

            existFooteLogo.IsDeleted = model.IsDeleted;
            existFooteLogo.LogoTitle = model.LogoTitle;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var existFooteLogo = await _dbContext.FooterLogo
                .Where(footerLogo => footerLogo.Id == id)
                .FirstOrDefaultAsync();

            if (existFooteLogo == null) return BadRequest();

            _dbContext.FooterLogo.Remove(existFooteLogo);

            var path = Path.Combine(Constants.RootPath, "assets", "images", "footerlogo", existFooteLogo.LogoImgUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
