using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using ProjectPresents;
using ProjectPresents.Data;


namespace ASPShopBag.Services
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> PrepareDataBase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var services = scope.ServiceProvider;

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<Client>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                //Sazdavane na roles
                await SeedRolesAsync(roleManager);
                //sazdavane na SUPER ADMIN s vsi4kite mu roli
                await SeedSuperAdminAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }

            return app;
        }
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Client"));
        }

            public static async Task SeedSuperAdminAsync(UserManager<Client> userManager)
            {
                //Seed Default User
                var defaultUser = new Client
                {
                    UserName = "theadmin",
                    Email = "stoicho@gmail.com",
                    FirstName = "Stoicho",
                    LastName = "Trifonov",
                    PhoneNumber = "0888888888",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    var result = await userManager.CreateAsync(defaultUser, "123!@#Qwe");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(defaultUser, "Admin");                  
                    }
                }
            }
        }
    }
