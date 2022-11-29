using EDUHOME.DAL;
using EDUHOME.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.Controllers
{
    public class BlogsController : Controller
    {
        private readonly AppDbContext _dbContext;
        private int _prductCount;
        public BlogsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _prductCount = dbContext.Blogs.Count();
        }

        public async Task<IActionResult> Index()
        {
            var blogs =await _dbContext.Blogs
                .Where(blog=>!blog.IsDeleted)
                .Take(3)
                .ToListAsync();

            ViewBag.productcount = _prductCount;

            var model = new List<BlogViewModel>();

            foreach (var blog in blogs)
            {
                var blogModel = new BlogViewModel();
                blogModel.Id = blog.Id;
                blogModel.Title = blog.Title;
                blogModel.ImageUrl = blog.ImageUrl; 
                blogModel.CreatedAt =blog.CreatedAt;
                blogModel.Reply = blog.Reply;
                blogModel.Content = blog.Content;   

                model.Add(blogModel);
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)  return NotFound();  

            var blog = await _dbContext.Blogs
                .Where(blog=>blog.Id == id)
                .FirstOrDefaultAsync();

            if(blog == null) return NotFound(); 

            var model = new BlogViewModel
            {
                Id = blog.Id,   
                Title = blog.Title,
                Reply = blog.Reply,
                Content = blog.Content,
                ImageUrl = blog.ImageUrl,
                CreatedAt = blog.CreatedAt, 
            };   

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PastialBlogs(int skip )
        {
            if (skip >= _prductCount)
                return BadRequest();

            var blogs = await _dbContext.Blogs
                .Where(blog=>!blog.IsDeleted)
                .Skip(skip)
                .Take(3)
                .ToListAsync();

            var model = new List<BlogViewModel>();

            foreach (var item in blogs)
            {
                model.Add(new BlogViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    CreatedAt= item.CreatedAt,
                    Content = item.Content, 
                    ImageUrl = item.ImageUrl,   
                    Reply= item.Reply,  
                });
            }

            return PartialView("_BlogListPartial", model);
        }

        public IActionResult Partial()
        {
            return PartialView("_BlogItemPartial");
        }
    }
}
