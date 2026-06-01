using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using marsian_library.Models;
using Microsoft.EntityFrameworkCore;

namespace marsian_library.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Employee", "Reader", "Admin" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
        public static async Task SeedAdressAsync(IServiceProvider serviceProvider){
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            if (!await context.Addresses.AnyAsync()){
                var address = new Address{
                    City = "Forge World of Mars",
                    Street = "Main Street",
                    Building = "Palace 1",
                    ZipCode = "00-000",
                };
                await context.Addresses.AddAsync(address);
                await context.SaveChangesAsync();
            }
        }
        public static async Task SeedJobAsync(IServiceProvider serviceProvider){
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            if(!await context.Jobs.AnyAsync()){
                var job = new Job{
                    Name = "Director"
                };
            await context.Jobs.AddAsync(job);
            await context.SaveChangesAsync();
            }
        }

        //https://warhammer40k.fandom.com/wiki/Belisarius_Cawl
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider){
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            string adminEmail = "beliasariuscawl@mechanicus.com";
            string adminPassword = "ArchmagosDominus123";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null){
                var address = await context.Addresses.FirstOrDefaultAsync();
                var job = await context.Jobs.FirstOrDefaultAsync();

                var adminEmp = new Emp
                {
                    FirstName = "Belisarius",
                    LastName = "Cawl",
                    AddressId = address.Id,
                    JobId = job.Id,
                };

                await context.Emps.AddAsync(adminEmp);
                await context.SaveChangesAsync();

                var newAdminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    EmpId = adminEmp.Id,
                };
                IdentityResult result = await userManager.CreateAsync(newAdminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdminUser, "Admin");
                }
            }
        }           
        public static async Task SeedDepartmentsAsync(IServiceProvider serviceProvider){
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            if (!await context.Depts.AnyAsync()){
                var address = await context.Addresses.FirstOrDefaultAsync();
                var director = await context.Emps.FirstOrDefaultAsync();
                var dept = new Dept{
                    AddressId = address.Id,
                    DirectorId = director.Id,

                };
                await context.Depts.AddAsync(dept);
                await context.SaveChangesAsync();
            }
        }
    }
}