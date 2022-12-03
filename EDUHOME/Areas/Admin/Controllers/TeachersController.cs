using Allup.Data;
using EDUHOME.Areas.Admin.Data;
using EDUHOME.Areas.Admin.Models;
using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using NuGet.Protocol.Plugins;
using System.Xml.Linq;

namespace EDUHOME.Areas.Admin.Controllers
{
    public class TeachersController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public TeachersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var teachers = await _dbContext.Teachers
                .ToListAsync();

            return View(teachers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherCreateViewModel model)
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

            var unicalName = await model.Image.GenerateFile(Constants.TeacherPath);

            var teacher = new Teacher
            {
                Name = model.Name,
                Profession = model.Profession,
                About = model.About,
                Degree = model.Degree,
                Experience = model.Experience,
                Hobbies = model.Hobbies,
                Faculty = model.Faculty,
                Mail = model.Mail,
                Skype = model.Skype,
                Number = model.Number,
                Language = model.Language,
                Teamleader = model.Teamleader,
                Development = model.Development,
                Design = model.Design,
                Innovation = model.Innovation,
                Communication = model.Communication,
                Imageurl = unicalName,
            };

            await _dbContext.Teachers.AddAsync(teacher);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            var teacher = await _dbContext.Teachers
                .Where(teacher => teacher.Id == id)
                .FirstOrDefaultAsync();

            var model = new TeacherUpdateViewModel
            {
                Name = teacher.Name,
                Profession = teacher.Profession,
                About = teacher.About,
                Degree = teacher.Degree,
                Experience = teacher.Experience,
                Hobbies = teacher.Hobbies,
                Faculty = teacher.Faculty,
                Mail = teacher.Mail,
                Skype = teacher.Skype,
                Number = teacher.Number,
                Language = teacher.Language,
                Teamleader = teacher.Teamleader,
                Development = teacher.Development,
                Design = teacher.Design,
                Innovation = teacher.Innovation,
                Communication = teacher.Communication,
                Imageurl = teacher.Imageurl,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, TeacherUpdateViewModel model)
        {
            if (id == null) return BadRequest();

            var existTeacher = await _dbContext.Teachers
                .Where(teacher => teacher.Id == id)
                .FirstOrDefaultAsync();

            var viewModel = new TeacherUpdateViewModel
            {
                Name = existTeacher.Name,
                Profession = existTeacher.Profession,
                About = existTeacher.About,
                Degree = existTeacher.Degree,
                Experience = existTeacher.Experience,
                Hobbies = existTeacher.Hobbies,
                Faculty = existTeacher.Faculty,
                Mail = existTeacher.Mail,
                Skype = existTeacher.Skype,
                Number = existTeacher.Number,
                Language = existTeacher.Language,
                Teamleader = existTeacher.Teamleader,
                Development = existTeacher.Development,
                Design = existTeacher.Design,
                Innovation = existTeacher.Innovation,
                Communication = existTeacher.Communication,
                Imageurl = existTeacher.Imageurl,
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

                var path = Path.Combine(Constants.RootPath, "assets", "images", "teacher", existTeacher.Imageurl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalName = await model.Image.GenerateFile(Constants.TeacherPath);

                existTeacher.Imageurl = unicalName;
            }

            existTeacher.Name = model.Name;
            existTeacher.Profession = model.Profession;
            existTeacher.About = model.About;
            existTeacher.Degree = model.Degree;
            existTeacher.Experience = model.Experience;
            existTeacher.Hobbies = model.Hobbies;
            existTeacher.Faculty = model.Faculty;
            existTeacher.Mail = model.Mail;
            existTeacher.Skype = model.Skype;
            existTeacher.Number = model.Number;
            existTeacher.Language = model.Language;
            existTeacher.Teamleader = model.Teamleader;
            existTeacher.Development = model.Development;
            existTeacher.Design = model.Design;
            existTeacher.Innovation = model.Innovation;
            existTeacher.Communication = model.Communication;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var existTeacher = await _dbContext.Teachers
                .Where(teacher => teacher.Id == id)
                .FirstOrDefaultAsync();

            if (existTeacher == null) return BadRequest();

            _dbContext.Teachers.Remove(existTeacher);

            var path = Path.Combine(Constants.RootPath, "assets", "images", "teacher", existTeacher.Imageurl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
