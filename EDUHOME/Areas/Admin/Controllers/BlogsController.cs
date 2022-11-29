using Allup.Data;
using EDUHOME.Areas.Admin.Data;
using EDUHOME.Areas.Admin.Models;
using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace EDUHOME.Areas.Admin.Controllers
{
    public class BlogsController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public BlogsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var blogs = await _dbContext.Blogs
                .ToListAsync();

            return View(blogs);           
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateViewModel model)
        {
            if (!ModelState.IsValid) return View();

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

            var unicalName = await model.Image.GenerateFile(Constants.BlogPath);

            await _dbContext.Blogs.AddAsync(new Blog
            {
                Title = model.Title,
                Content= model.Content,
                Reply =model.Reply,
                CreatedAt = DateTime.Now,   
                ImageUrl = unicalName, 
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); 
        }

        public async Task<IActionResult> Update(int? id)
        {

            if (id == null) return BadRequest();

            var blog = await _dbContext.Blogs
                .Where(blog => blog.Id == id)
                .FirstOrDefaultAsync();

            if (blog == null) return BadRequest();

            var model = new BlogUpdateViewModel
            {
                Title = blog.Title,
                Content = blog.Content, 
                Reply=blog.Reply,
                ImageUrl= blog.ImageUrl,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id , BlogUpdateViewModel model)
        {
            if (id == null) return BadRequest();

            var existBlog = await _dbContext.Blogs
                .Where(blog => blog.Id == id)
                .FirstOrDefaultAsync();

            var viewModel = new BlogUpdateViewModel
            {
                Title = existBlog.Title,
                Content = existBlog.Content,
                Reply = existBlog.Reply,
                ImageUrl = existBlog.ImageUrl,
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

                var path = Path.Combine(Constants.RootPath, "assets", "images", "blog", existBlog.ImageUrl);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalName = await model.Image.GenerateFile(Constants.BlogPath);

                existBlog.ImageUrl = unicalName;
            }

            existBlog.Title = model.Title;
            existBlog.Content = model.Content;  
            existBlog.Reply = model.Reply;  

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var existBlog = await _dbContext.Blogs
               .Where(blog => blog.Id == id)
               .FirstOrDefaultAsync();

            if (existBlog == null) return BadRequest();

            _dbContext.Blogs.Remove(existBlog);

            var path = Path.Combine(Constants.RootPath, "assets", "images", "blog", existBlog.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
