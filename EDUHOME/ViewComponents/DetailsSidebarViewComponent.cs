using EDUHOME.DAL;
using EDUHOME.DAL.Entities;
using EDUHOME.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.ViewComponents
{
    public class DetailsSidebarViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public DetailsSidebarViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _dbContext.Categories
                .Where(category=>!category.IsDeleted)
                .Include(x=>x.Courses)
                .ToListAsync();

            var blogs = await _dbContext.Blogs
                .Where(blog => !blog.IsDeleted)
                .OrderByDescending(blog => blog.Id)
                .Take(3)
                .ToListAsync();

            var model = new DetailsSidebarViewModel
            {
                Categories = categories,
                Blogs = blogs,
            };

            return View(model);
        }
    }
}
