using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using EDUHOME.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _dbContext;
 
        public CoursesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var course = await _dbContext.Courses
                .Where(course => !course.IsDeleted)
                .ToListAsync();

            var model = new List<CourseViewModel>();

            course.ForEach(course => model.Add(new CourseViewModel
            {
                Id = course.Id,
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
                Fee= course.Fee,
                ImageUrl = course.ImageUrl,

            }));

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var course = await _dbContext.Courses
                .Where(blog => blog.Id == id)
                .FirstOrDefaultAsync();

            if (course == null) return NotFound();

            var model = new CourseViewModel
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
            };    
      
            return View(model);
        }

        public async Task<IActionResult> Search(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return NoContent();

            var courses = await _dbContext.Courses
                .Where(course=>!course.IsDeleted && course.Name.ToLower().Contains(searchText.ToLower()))
                .ToListAsync();

            var model = new List<CourseViewModel>();

            courses.ForEach(course => model.Add(new CourseViewModel
            {
                Id = course.Id,
                Name = course.Name,
                ImageUrl = course.ImageUrl,
            }));           

            return PartialView("_CourseListPartial", courses);
        }
    }
}
