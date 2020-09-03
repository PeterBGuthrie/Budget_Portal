namespace Budget_Portal.Migrations
{
    using Budget_Portal.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<Budget_Portal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Budget_Portal.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            #region Role Creation
            var roleManager = new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(context));

            // Super user
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            // Represents the head of houseHold
            if (!context.Roles.Any(r => r.Name == "Head"))
            {
                roleManager.Create(new IdentityRole { Name = "Head" });
            }

            // A user who is part of a Household, but is not in the head of HouseHold role
            // We will assign this role to any user that registers with an invitation code or who enters an invitation code to join a household
            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                roleManager.Create(new IdentityRole { Name = "Member" });
            }

            // Represents a new user who has not joined or created a household
            // We will assign this role to anyone who registers an account without using an invitation code
            // This role will be assigned to nay user that leaves a household
            if (!context.Roles.Any(r => r.Name == "New User"))
            {
                roleManager.Create(new IdentityRole { Name = "New User" });
            }
            #endregion

            #region User creation
            var AdminEmail = WebConfigurationManager.AppSettings["AdminEmail"];
            var AdminPassword = WebConfigurationManager.AppSettings["AdminPassword"];
            var AdminFirstName = WebConfigurationManager.AppSettings["AdminFirstName"];
            var AdminLastName = WebConfigurationManager.AppSettings["AdminLastName"];
            var DemoPassword = WebConfigurationManager.AppSettings["DemoPassword"];
            var DemoHeadEmail = WebConfigurationManager.AppSettings["DemoHeadEmail"];
            var DemoMemberEmail = WebConfigurationManager.AppSettings["DemoMemberEmail"];
            var DemoNUserEmail = WebConfigurationManager.AppSettings["DemoNUserEmail"];

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == AdminEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = AdminEmail,
                    UserName = AdminEmail,
                    FirstName = AdminFirstName,
                    LastName = AdminLastName,
                }, AdminPassword);
                var userId = userManager.FindByEmail(AdminEmail).Id;
                userManager.AddToRole(userId, "Admin");
            }

            if (!context.Users.Any(u => u.Email == DemoHeadEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = DemoHeadEmail,
                    UserName = DemoHeadEmail,
                    FirstName = "DemoHeadJohn",
                    LastName = "Smith",
                }, DemoPassword);
                var userId = userManager.FindByEmail(DemoHeadEmail).Id;
                userManager.AddToRole(userId, "Head");
            }

            if (!context.Users.Any(u => u.Email == DemoMemberEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = DemoMemberEmail,
                    UserName = DemoMemberEmail,
                    FirstName = "DemoMemPhilip",
                    LastName = "Stump",
                }, DemoPassword);
                var userId = userManager.FindByEmail(DemoMemberEmail).Id;
                userManager.AddToRole(userId, "Member");
            }

            if (!context.Users.Any(u => u.Email == DemoNUserEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = DemoNUserEmail,
                    UserName = DemoNUserEmail,
                    FirstName = "DemoHeadJane",
                    LastName = "Philips",
                }, DemoPassword);
                var userId = userManager.FindByEmail(DemoNUserEmail).Id;
                userManager.AddToRole(userId, "New User");
            }
            #endregion
        }
    }
}
