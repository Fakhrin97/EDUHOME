using EDUHOME.Areas.Admin.Models;
using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.Areas.Admin.Controllers
{
    public class FooterSocialMediasController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public FooterSocialMediasController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var footerSocialMedia = await _dbContext.FooterSocialMedias
                .ToListAsync();

            return View(footerSocialMedia);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FooterSocialMediaCreateViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var existIcon = await _dbContext.FooterSocialMedias
                .Where(icon => icon.Icon == model.Icon)
                .FirstOrDefaultAsync();

            if (existIcon != null)
            {
                ModelState.AddModelError("icon", "Bu Icon Artqi Bazada Movcuddur");
                return View();
            }

            await _dbContext.FooterSocialMedias.AddAsync(new FooterSocialMedia
            {
                Icon = model.Icon,
                Link = model.Link,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var socialMediaIcon = await _dbContext.FooterSocialMedias
                .Where(icon => icon.Id == id)
                .FirstOrDefaultAsync();

            var model = new FooterSocialMediaUpdateViewModel
            {
                Id = id,
                Icon = socialMediaIcon.Icon,
                Link = socialMediaIcon.Link,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, FooterSocialMediaUpdateViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var existFooterIcon = await _dbContext.FooterSocialMedias
                 .Where(icon => icon.Id == id)
                 .FirstOrDefaultAsync();

            var result = await _dbContext.FooterSocialMedias
                .Where(info => info.Icon == model.Icon)
                .FirstOrDefaultAsync();

            if (existFooterIcon.Icon != model.Icon)
            {
                if (result != null)
                {
                    ModelState.AddModelError("icon", "Bu adda artiq icon movcuddur");
                    return View();
                }
            }

            existFooterIcon.Icon = model.Icon;
            existFooterIcon.Link = model.Link;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existFooterIcon = await _dbContext.FooterSocialMedias
                .Where(info => info.Id == id)
                .FirstOrDefaultAsync();

            if (existFooterIcon == null) return BadRequest();

            _dbContext.FooterSocialMedias.Remove(existFooterIcon);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
