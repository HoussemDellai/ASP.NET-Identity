using System;
using System.Linq;
using AspNetIdentity.WebApi.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentity.WebApi.Helpers
{
    public class AccountsHelpers
    {
        public void CreateSuperAdminAndRoles()
        {
            var manager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "SuperPowerUser",
                Email = "houssem.dellai@live.com",
                EmailConfirmed = true,
                FirstName = "Houssem",
                LastName = "Dellai",
                Level = 1,
                JoinDate = DateTime.Now.AddYears(-3)
            };

            manager.Create(user, "P@ssword!");

            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole {Name = "SuperAdmin"});
                roleManager.Create(new IdentityRole {Name = "Admin"});
                roleManager.Create(new IdentityRole {Name = "User"});
            }

            var adminUser = manager.FindByName("SuperPowerUser");

            manager.AddToRoles(adminUser.Id, "SuperAdmin", "Admin");
        }
    }
}