using EDUHOME.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EDUHOME.DAL
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Information> Informations { get; set; }
        public DbSet<UsefulLink> UsefulLinks { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<FooterLogo> FooterLogo { get; set; }
        public DbSet<FooterSocialMedia> FooterSocialMedias { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}
