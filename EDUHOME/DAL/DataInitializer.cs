using Allup.Data;
using EDUHOME.DAL.Entities;
using EDUHOME.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EDUHOME.DAL
{
    public class DataInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _dbContext;
        private readonly AdminUser _adminUser;

        public DataInitializer(IServiceProvider serviceProvider)
        {
            _adminUser = serviceProvider.GetService<IOptions<AdminUser>>().Value;
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        }

        public async Task SeedData()
        {
            await _dbContext.Database.MigrateAsync();

            var role = Constants.AdminRole;

            if (!await _roleManager.RoleExistsAsync(role))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole { Name = role });

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }

            }

            var existUser = await _userManager.FindByNameAsync(_adminUser.UserName);

            if (existUser != null) return;

            var userCrateResult = await _userManager.CreateAsync(new User
            {
                UserName = _adminUser.UserName,
                FristName = _adminUser.FristName,
                LastName = _adminUser.LastName,
                Email = _adminUser.Email,
            },_adminUser.Password);

            if (!userCrateResult.Succeeded)
            {
                foreach (var error in userCrateResult.Errors)
                {
                    Console.WriteLine(error.Description);
                }
            }
            else
            {
                var existCratedUser = await _userManager.FindByNameAsync(_adminUser.UserName);

                await _userManager.AddToRoleAsync(existCratedUser, Constants.AdminRole);
            }
        }
    }
}
