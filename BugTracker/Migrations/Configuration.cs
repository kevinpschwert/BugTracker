namespace BugTracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(context));
            //Admin

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            var userManager = new UserManager<ApplicationUser>(
             new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "kevinpschwert@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "kevinpschwert@gmail.com",
                    Email = "kevinpschwert@gmail.com",
                    FirstName = "Kevin",
                    LastName = "Schwert",
                    DisplayName = "kschwert",
                    EmailConfirmed = true
                }, "1th@nkGod!");
            }

            var userId = userManager.FindByEmail("kevinpschwert@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            //Project Manager

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            if (!context.Users.Any(u => u.Email == "jtwichell@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "jtwichell@coderfoundry.com",
                    Email = "jtwichell@coderfoundry.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                    DisplayName = "MadDog",
                    EmailConfirmed = true
                }, "Abc123!");
            }           

            var userId1 = userManager.FindByEmail("jtwichell@coderfoundry.com").Id;
            userManager.AddToRole(userId1, "Project Manager");

            //Developer

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Users.Any(u => u.Email == "mjaang@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "mjaang@coderfoundry.com",
                    Email = "mjaang@coderfoundry.com",
                    FirstName = "Mark",
                    LastName = "Jaang",
                    DisplayName = "MarkyMark",
                    EmailConfirmed = true
                }, "Abc123!");
            }

            var userId2 = userManager.FindByEmail("mjaang@coderfoundry.com").Id;
            userManager.AddToRole(userId1, "Developer");

            //Submitter

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            if (!context.Users.Any(u => u.Email == "mike@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "mike@coderfoundry.com",
                    Email = "mike@coderfoundry.com",
                    FirstName = "Mike",
                    LastName = "Shelton",
                    DisplayName = "Mikey",
                    EmailConfirmed = true
                }, "Abc123!");
            }

            var userId3 = userManager.FindByEmail("mike@coderfoundry.com").Id;
            userManager.AddToRole(userId1, "Submitter");

            //Guest Admin

            if (!context.Users.Any(u => u.Email == "guestadmin@bugtracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "guestadmin@bugtracker.com",
                    Email = "guestadmin@bugtracker.com",
                    FirstName = "Guest",
                    LastName = "Admin",
                    DisplayName = "GuestAdmin",
                    EmailConfirmed = true
                }, "Abc123!");
            }

            var userId4 = userManager.FindByEmail("guestadmin@bugtracker.com").Id;
            userManager.AddToRole(userId4, "Admin");

            //Guest PM

            if (!context.Users.Any(u => u.Email == "guestPM@bugtracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "guestPM@bugtracker.com",
                    Email = "guestPM@bugtracker.com",
                    FirstName = "Guest",
                    LastName = "ProjectManager",
                    DisplayName = "GuestPM",
                    EmailConfirmed = true
                }, "Abc123!");
            }

            var userId5 = userManager.FindByEmail("guestPM@bugtracker.com").Id;
            userManager.AddToRole(userId5, "Project Manager");

            //Guest Developer

            if (!context.Users.Any(u => u.Email == "guestdeveloper@bugtracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "guestdeveloper@bugtracker.com",
                    Email = "guestdeveloper@bugtracker.com",
                    FirstName = "Guest",
                    LastName = "Developer",
                    DisplayName = "GuestDeveloper",
                    EmailConfirmed = true
                }, "Abc123!");
            }

            var userId6 = userManager.FindByEmail("guestdeveloper@bugtracker.com").Id;
            userManager.AddToRole(userId6, "Developer");

            //Guest Submitter

            if (!context.Users.Any(u => u.Email == "guestsubmitter@bugtracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "guestsubmitter@bugtracker.com",
                    Email = "guestsubmitter@bugtracker.com",
                    FirstName = "Guest",
                    LastName = "Submitter",
                    DisplayName = "GuestSubmitter",
                    EmailConfirmed = true
                }, "Abc123!");
            }

            var userId7 = userManager.FindByEmail("guestsubmitter@bugtracker.com").Id;
            userManager.AddToRole(userId7, "Submitter");



            //Begin Roles           

            if (!context.TicketPriorities.Any(u => u.Name == "High"))
            { context.TicketPriorities.Add(new TicketPriority { Name = "High" }); }

            if (!context.TicketPriorities.Any(u => u.Name == "Medium"))
            { context.TicketPriorities.Add(new TicketPriority { Name = "Medium" }); }

            if (!context.TicketPriorities.Any(u => u.Name == "Low" ))
             { context.TicketPriorities.Add(new TicketPriority { Name = "Low" }); }

            if (!context.TicketPriorities.Any(u => u.Name == "Urgent"))
            { context.TicketPriorities.Add(new TicketPriority { Name = "Urgent" }); }

            if (!context.TicketTypes.Any(u => u.Name == "Project Task"))
            { context.TicketTypes.Add(new TicketType { Name = "Project Task" }); }

            if (!context.TicketTypes.Any(u => u.Name == "Software Update"))
            { context.TicketTypes.Add(new TicketType { Name = "Software Update" }); }

            if (!context.TicketStatuses.Any(u => u.Name == "New"))
            { context.TicketStatuses.Add(new TicketStatus { Name = "New" }); }

            if (!context.TicketStatuses.Any(u => u.Name == "In Development"))
            { context.TicketStatuses.Add(new TicketStatus { Name = "In Development" }); }

            if (!context.TicketStatuses.Any(u => u.Name == "Completed"))
            { context.TicketStatuses.Add(new TicketStatus { Name = "Completed" }); }

            // high, medium, urget, production fix, project task, software update, new, In Development, Completed
        }
    }
}
