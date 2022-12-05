using Allup.Data;
using EDUHOME.Areas.Admin.Data;
using EDUHOME.Areas.Admin.Models;
using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System.Xml.Linq;

namespace EDUHOME.Areas.Admin.Controllers
{
    public class CoursesController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public CoursesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var courses = await _dbContext.Courses
                .Include(category => category.Category)
                .ToListAsync();

            return View(courses);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _dbContext.Categories
                .Where(categorie => !categorie.IsDeleted)
                .ToListAsync();

            var categoriesSelectedListItem = new List<SelectListItem>();

            categories.ForEach(category => categoriesSelectedListItem.Add(new SelectListItem(category.Name, category.Id.ToString())));

            var model = new CourseCreateViewModel
            {
                Categories = categoriesSelectedListItem,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateViewModel model)
        {
            var categories = await _dbContext.Categories
                .Where(categorie => !categorie.IsDeleted)
                .Include(categorie => categorie.Courses)
                .ToListAsync();

            var categoriesSelectedListItem = new List<SelectListItem>();

            categories.ForEach(category => categoriesSelectedListItem.Add(new SelectListItem(category.Name, category.Id.ToString())));

            var viewModel = new CourseCreateViewModel
            {
                Categories = categoriesSelectedListItem,
            };

            if (!ModelState.IsValid) return View(viewModel);

            var ecistCourse = await _dbContext.Courses
                .Where(course => course.Name == model.Name)
                .FirstOrDefaultAsync();

            if (ecistCourse != null)
            {
                ModelState.AddModelError("", "Bu Add Artiq Kurs Movcuddur");
                return View(viewModel);
            }

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

            var unicalName = await model.Image.GenerateFile(Constants.CoursePath);

            var course = new Course
            {
                Name = model.Name,
                Content = model.Content,
                About = model.About,
                Apply = model.Apply,
                Certification = model.Certification,
                Reply = model.Reply,
                GeginningTime = model.GeginningTime,
                Duration = model.Duration,
                ClassDuration = model.ClassDuration,
                Level = model.Level,
                Language = model.Language,
                StudentsCount = model.StudentsCount,
                Assesments = model.Assesments,
                Fee = model.Fee,
                ImageUrl = unicalName,
                CategoryId = model.CategoryId,
            };

            await _dbContext.Courses.AddAsync(course);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var categories = await _dbContext.Categories
                .Where(categorie => !categorie.IsDeleted)
                .ToListAsync();

            var categoriesSelectedListItem = new List<SelectListItem>();

            categories.ForEach(category => categoriesSelectedListItem.Add(new SelectListItem(category.Name, category.Id.ToString())));

            var course = await _dbContext.Courses
                .Where(course => course.Id == id)
                .FirstOrDefaultAsync();

            var model = new CourseUpdateViewModel
            {
                Name = course.Name,
                Content = course.Content,
                About = course.About,
                Apply = course.Apply,
                Certification = course.Certification,
                Reply = course.Reply,
                GeginningTime = course.GeginningTime,
                Duration = course.Duration,
                ClassDuration = course.ClassDuration,
                Level = course.Level,
                Language = course.Language,
                StudentsCount = course.StudentsCount,
                Assesments = course.Assesments,
                Fee = course.Fee,
                ImageUrl = course.ImageUrl,
                CategoryId = course.CategoryId,
                Categories = categoriesSelectedListItem,
                IsDeleted = course.IsDeleted,

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CourseUpdateViewModel model)
        {
            if (id == null) return BadRequest();

            var existCourse = await _dbContext.Courses
                .Where(course => course.Id == id)
                .FirstOrDefaultAsync();

            if (existCourse == null) return BadRequest();

            var categories = await _dbContext.Categories
                .Where(categorie => !categorie.IsDeleted)
                .ToListAsync();

            var categoriesSelectedListItem = new List<SelectListItem>();

            categories.ForEach(category => categoriesSelectedListItem.Add(new SelectListItem(category.Name, category.Id.ToString())));

            var viewModel = new CourseUpdateViewModel
            {
                Categories = categoriesSelectedListItem,
                ImageUrl = existCourse.ImageUrl,
            };

            if (!ModelState.IsValid) return View(viewModel);

            if (existCourse.Name != model.Name)
            {
                var ecistCourse = await _dbContext.Courses
                    .Where(course => course.Name == model.Name)
                    .FirstOrDefaultAsync();

                if (ecistCourse != null)
                {
                    ModelState.AddModelError("", "Bu Add Artiq Kurs Movcuddur");
                    return View(viewModel);
                }
            }

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

                var path = Path.Combine(Constants.RootPath, "assets", "images", "course", existCourse.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalName = await model.Image.GenerateFile(Constants.CoursePath);

                existCourse.ImageUrl = unicalName;
            }

            existCourse.IsDeleted = model.IsDeleted;
            existCourse.Name = model.Name;
            existCourse.Content = model.Content;
            existCourse.About = model.About;
            existCourse.Apply = model.Apply;
            existCourse.Certification = model.Certification;
            existCourse.Reply = model.Reply;
            existCourse.GeginningTime = model.GeginningTime;
            existCourse.Duration = model.Duration;
            existCourse.ClassDuration = model.ClassDuration;
            existCourse.Level = model.Level;
            existCourse.Language = model.Language;
            existCourse.StudentsCount = model.StudentsCount;
            existCourse.Assesments = model.Assesments;
            existCourse.Fee = model.Fee;
            existCourse.CategoryId = model.CategoryId;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var existCourse = await _dbContext.Courses
                .Where(course => course.Id == id)
                .FirstOrDefaultAsync();

            if (existCourse == null) return BadRequest();

            _dbContext.Courses.Remove(existCourse);

            var path = Path.Combine(Constants.RootPath, "assets", "images", "course", existCourse.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
