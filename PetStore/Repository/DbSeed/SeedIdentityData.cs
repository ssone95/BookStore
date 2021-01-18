using PetStore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace PetStore.Repository.DbSeed
{
    public static class SeedIdentityData
    {
        private const string adminUser = "Admin";
        private const string adminPass = "Secret123$";

        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            IdentityContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<IdentityContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            UserManager<IdentityUser> userManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();


            IdentityUser user = await userManager.FindByIdAsync(adminUser);
            if(user == null)
            {
                user = new IdentityUser(adminUser);
                user.Email = "admin@test.com";
                user.PhoneNumber = "123-1234";

                await userManager.CreateAsync(user, adminPass);
            }
        }
    }
}
