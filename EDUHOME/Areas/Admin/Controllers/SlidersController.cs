using Allup.Data;
using EDUHOME.Areas.Admin.Data;
using EDUHOME.Areas.Admin.Models;
using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.Areas.Admin.Controllers
{
    public class SlidersController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public SlidersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _dbContext.Sliders
                .ToListAsync();

            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateViewModel model)
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

            var unicalName = await model.Image.GenerateFile(Constants.SliderPath);

            await _dbContext.Sliders.AddAsync(new Slider
            {
                Imageurl = unicalName,
                Header = model.Header,
                Mainheader = model.Mainheader,
                Content = model.Content,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            var slider = await _dbContext.Sliders
                .Where(slider => slider.Id == id)
                .FirstOrDefaultAsync();

            if (slider == null) return BadRequest();

            var model = new SliderUpdateViewModel
            {
                Mainheader = slider.Mainheader,
                Content = slider.Content,
                Header = slider.Header,
                ImageUrl = slider.Imageurl,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SliderUpdateViewModel model)
        {
            if (id == null) return BadRequest();

            var existSlider = await _dbContext.Sliders
                .Where(slider => slider.Id == id)
                .FirstOrDefaultAsync();

            var viewModel = new SliderUpdateViewModel
            {
                Mainheader = existSlider.Mainheader,
                Content = existSlider.Content,
                Header = existSlider.Header,
                ImageUrl = existSlider.Imageurl,
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

                var path = Path.Combine(Constants.RootPath,"assets" ,"images","slider", existSlider.Imageurl);

                if(System.IO.File.Exists(path))
                    System.IO.File.Delete(path); 

                var unicalName = await model.Image.GenerateFile(Constants.SliderPath);

                existSlider.Imageurl = unicalName;  
            }

            existSlider.Mainheader = model.Mainheader;
            existSlider.Content = model.Content;
            existSlider.Header = model.Header;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var existSlider = await _dbContext.Sliders
                .Where(slider => slider.Id == id)
                .FirstOrDefaultAsync();

            if (existSlider == null) return BadRequest();

            _dbContext.Sliders.Remove(existSlider);

            var path = Path.Combine(Constants.RootPath, "assets", "images", "slider", existSlider.Imageurl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            await _dbContext.SaveChangesAsync();    

            return RedirectToAction(nameof(Index));
        }

    }
}
