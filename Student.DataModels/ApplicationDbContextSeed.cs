using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Student.DataModels
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Teacher", "CourseManager", "SuperAdmin" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var email = "superadmin@gmail.com";
            var user = await userManager.FindByEmailAsync(email);
            if (user != null && !await userManager.IsInRoleAsync(user, "SuperAdmin"))
            {
                await userManager.AddToRoleAsync(user, "SuperAdmin");
            }
        }

    }
}
