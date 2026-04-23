using ECommerce.DAL;
using Microsoft.AspNetCore.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager)
    {
        await SeedRolesAsync(roleManager);
        await SeedAdminAsync(userManager);
    }

    private static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
    {
        string[] roles = { "User", "Admin" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new AppRole { Name = role });
            }
        }
    }

    private static async Task SeedAdminAsync(UserManager<AppUser> userManager)
    {
        var email = "admin@mail.com";

        if (await userManager.FindByEmailAsync(email) == null)
        {
            var admin = new AppUser
            {
                UserName = email,
                Email = email,
                FullName = "Admin"
            };

            await userManager.CreateAsync(admin, "1234");
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}