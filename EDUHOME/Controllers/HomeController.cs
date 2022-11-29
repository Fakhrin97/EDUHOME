using EDUHOME.DAL;
using EDUHOME.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel();
            var slideres = await _dbContext.Sliders
                .ToListAsync();

            var blogs = await _dbContext.Blogs
                .Where(blog => !blog.IsDeleted)
                .Take(3)
                .ToListAsync();

            var courses = await _dbContext.Courses
                .Where(course=>!course.IsDeleted)
                .Take(3)
                .ToListAsync();

            var blogModelList = new List<BlogViewModel>();
            var courseList = new List<CourseViewModel>();

            courses.ForEach(course => courseList.Add(new CourseViewModel
            {
                Id = course.Id,
                Name = course.Name,
                About = course.About,
                ImageUrl = course.ImageUrl, 
            }));

            foreach (var blog in blogs)
            {
                var blogModel = new BlogViewModel();
                blogModel.Id = blog.Id;
                blogModel.Title = blog.Title;
                blogModel.ImageUrl = blog.ImageUrl;
                blogModel.CreatedAt = blog.CreatedAt;
                blogModel.Reply = blog.Reply;
                blogModel.Content = blog.Content;

                blogModelList.Add(blogModel);
            }

            model.Sliders = slideres;  
            model.Blogs = blogModelList;
            model.Courses = courseList;

            return View(model);
        }

       
    }
}