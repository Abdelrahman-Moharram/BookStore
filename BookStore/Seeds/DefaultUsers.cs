using BookStore.Constants;
using BookStore.DTOs.Account;
using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BookStore.Seeds
{
    public static class DefaultUsers
    {
        
        


        public static async Task SeedBasicAsync(IAuthService authService)
        {
            var user = await authService.AddUser(new RegisterDTO
            {
                Email = "basic @site.com",
                Password = "12345678",
                PhoneNumber = "01000000000",
                Username = "basic",
            });

            if (user != null)
            {
                await authService.AddUserToRole(user, Roles.Basic.ToString());
            }

        }


        public static async Task SeedAdminAsync(IAuthService authService, RoleManager<IdentityRole> roleManager)
        {
            var user = await authService.AddUser(new RegisterDTO
            {
                Email = "admin@site.com",
                Password = "12345678",
                PhoneNumber = "01000000000",
                Username = "Admin",
            });

            if (user != null)
            {
                foreach (string role in Enum.GetNames(typeof(Roles)))
                {
                    if (role != Roles.SuperAdmin.ToString())
                        await authService.AddUserToRole(user, role);
                }
            }

            // Seed Claims
            foreach (string module in Enum.GetNames(typeof(Modules)))
            {
                var cruds = Enum.GetNames(typeof(Cruds)).Cast<string>().ToArray();
                await roleManager.AddModuleCliamsForRole(Roles.Admin.ToString(), module, cruds);
            }
        }

        public static async Task SeedSuperAdminAsync(IAuthService authService, RoleManager<IdentityRole> roleManager)
        {
            var user = await authService.AddUser(new RegisterDTO
            {
                Email = "superadmin@site.com",
                Password = "12345678",
                PhoneNumber = "01000000000",
                Username = "Super-Admin",
            });

            if (user != null)
            {
                foreach (string role in Enum.GetNames(typeof(Roles)))
                {
                    await authService.AddUserToRole(user, role);
                }
            }
            // Seed Claims
            foreach (string module in Enum.GetNames( typeof(Modules) ))
            {
                var cruds = Enum.GetNames(typeof(Cruds)).Cast<string>().ToArray();
                await roleManager.AddModuleCliamsForRole(Roles.SuperAdmin.ToString(), module, cruds);
            }

        }


        




    }
}
