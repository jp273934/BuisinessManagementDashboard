using Microsoft.AspNetCore.Identity;

namespace BuisinessManagementDashboard.Data
{
    public class IdentitySeeder
    {
        public static async Task SeedAsync(
            IServiceProvider services,
            string adminEmail)
        {
            var roleManager =
                services.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                services.GetRequiredService<UserManager<IdentityUser>>();

            const string adminRole = "Admin";

            // Create the Admin role if it doesn't exist.
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                var roleResult = await roleManager.CreateAsync(
                    new IdentityRole(adminRole));

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Could not create the Admin role.");
                }
            }

            // Find an existing user. Never create a default
            // administrator account with a hardcoded password.
            var user = await userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                throw new InvalidOperationException(
                    "Register the administrator account before seeding its role.");
            }

            if (!await userManager.IsInRoleAsync(user, adminRole))
            {
                var result = await userManager.AddToRoleAsync(
                    user, adminRole);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Could not assign the Admin role.");
                }
            }
        }
    }
}
