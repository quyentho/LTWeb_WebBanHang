using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Data;
using WebBanHang.Models;

namespace WebBanHang.Utility
{
    public static class ApplicationDbInitializer
    {
        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByEmailAsync("admin@user.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "admin@user.com",
                    Email = "admin@user.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "qwer1234@").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
