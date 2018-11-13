using Microsoft.AspNetCore.Identity;
using SantaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SantaAPI.Data
{
    public class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context,
                             UserManager<ApplicationUser> userManager)
        {
            context.Database.EnsureCreated();

            string password = "P@$$w0rd";



            if (await userManager.FindByNameAsync("santa") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "santa",
                    Email = "santa@np.com",
                    FirstName = "Santa",
                    LastName = "Clause",
                    Street = "Elfo 1",
                    City = "NorthPole",
                    Province = "NP",
                    PostalCode = "H0H 0H0",
                    Country = "Canada",
                    DateCreated = DateTime.Now
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            if (await userManager.FindByNameAsync("tim") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "tim",
                    Email = "tim@np.com",
                    DateCreated = DateTime.Now
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, "Child");
                }
            }


        }
    }
}
