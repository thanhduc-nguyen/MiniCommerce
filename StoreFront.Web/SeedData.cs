using Microsoft.AspNetCore.Identity;
using StoreFront.Web.Models.Account;
using System.Security.Claims;

namespace StoreFront.Web;

public static class SeedData
{
    public static async Task AddSampleRoles(RoleManager<IdentityRole> roleManager)
    {
        foreach (var roleName in new[] { Constants.AdminRole, Constants.CustomerRole })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    public static async Task AddSampleUsers(UserManager<ApplicationUser> userManager)
    {
        await CreateUserAsync(
            userManager,
            userName: "admin",
            email: "admin@minicommerce.local",
            name: "MiniCommerce Admin",
            roleName: Constants.AdminRole);

        await CreateUserAsync(
            userManager,
            userName: "customer",
            email: "customer@minicommerce.local",
            name: "Sample Customer",
            roleName: Constants.CustomerRole);
    }

    private static async Task CreateUserAsync(
        UserManager<ApplicationUser> userManager,
        string userName,
        string email,
        string name,
        string roleName)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user is not null)
        {
            return;
        }

        user = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            Name = name,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, "Cvbnm123@");
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(error => error.Description)));
        }

        await userManager.AddToRoleAsync(user, roleName);
        await userManager.AddClaimsAsync(user, new[]
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Email, email)
        });
    }
}
