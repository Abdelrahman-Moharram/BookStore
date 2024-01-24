using BookStore.Constants;
using BookStore.DTOs.Account;
using BookStore.Helpers;
using BookStore.Services;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Seeds
{
    public static class DefaultUsers
    {
        
        


        public static async Task SeedBasicAsync(IAuthService authService, IRoleService roleService, RoleManager<IdentityRole> roleManager)
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
                await roleService.AddUserToRole(user, Roles.Basic.ToString());
            }
            foreach (string module in Enum.GetNames(typeof(Modules)))
            {
                // get cruds
                var cruds = RoleModules.instance.cruds(roleName: Roles.Basic.ToString(), Module: module);

                await roleManager.AddModuleCliamsForRole(Roles.Basic.ToString(), module, cruds);
            }
        }


        public static async Task SeedAdminAsync(IAuthService authService, IRoleService roleService, RoleManager<IdentityRole> roleManager)
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
                        await roleService.AddUserToRole(user, role);
                }
            }

            // Seed Claims
            foreach (string module in Enum.GetNames(typeof(Modules)).ToArray())
            {
                // get cruds
                var cruds = RoleModules.instance.cruds(roleName: Roles.Admin.ToString(), Module: module);

                await roleManager.AddModuleCliamsForRole(Roles.Admin.ToString(), module, cruds);
            }
        }

        public static async Task SeedSuperAdminAsync(IAuthService authService, IRoleService roleService, RoleManager<IdentityRole> roleManager)
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
                    await roleService.AddUserToRole(user, role);
                }
            }
            // Seed Claims
            foreach (string module in Enum.GetNames( typeof(Modules) ))
            {
                // get cruds
                var cruds = RoleModules.instance.cruds(roleName: Roles.SuperAdmin.ToString(), Module: module);

                await roleManager.AddModuleCliamsForRole(Roles.SuperAdmin.ToString(), module, cruds);
            }

        }


        




    }
}
