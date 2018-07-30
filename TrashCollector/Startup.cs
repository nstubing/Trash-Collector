using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Data.Entity;
using System.Linq;
using TrashCollector.Models;

[assembly: OwinStartupAttribute(typeof(TrashCollector.Startup))]
namespace TrashCollector
{
    public partial class Startup
    {
        ApplicationDbContext context;

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            context = new ApplicationDbContext();
            CreateRolesandUsers();
            PopulatePickups();
            AddOneTimePickups();
        }

        private void PopulatePickups()
        {
            var currentDay = DateTime.Now;
            var currentDayString = currentDay.ToString("d");
            var istodayPopulated = context.Pickups.FirstOrDefault(p => p.date == currentDayString);
            if(istodayPopulated==null)
            {
                var customerID = context.Roles.FirstOrDefault(r => r.Name == "Customer").Id;
                var DailyUserRoles = context.Roles.SelectMany(u => u.Users.Where(r => r.RoleId == customerID));
                var customersUsers = from userID in DailyUserRoles
                                     join user in context.Users
                                     on userID.UserId equals user.Id
                                     where userID.UserId == user.Id
                                     select user;
                var dayPickups = customersUsers.Where(u => u.ScheduledDay == currentDay.DayOfWeek.ToString());
                var dayMinusExcluded = dayPickups.Where(u => u.ExcludedStartDate == null || u.ExcludedStartDate > currentDay || u.ExcludedEndDate < currentDay).Include(u=>u);
                var OneTimers = context.OneTimePickups.Where(o => o.date == currentDayString);
                foreach (ApplicationUser customer in dayMinusExcluded)
                {
                    Pickup newPickup = new Pickup();
                    newPickup.User = customer;
                    newPickup.date = currentDay.ToString("d");
                    newPickup.Confirmation = "Unconfirmed";
                    context.Pickups.Add(newPickup);
                }
                foreach (OneTimePickup customer in OneTimers)
                {
                    Pickup newPickup = new Pickup();
                    newPickup.User = customer.User;
                    newPickup.date = customer.date;
                    newPickup.Confirmation = "Unconfirmed";
                    context.Pickups.Add(newPickup);
                }
                context.SaveChanges();
            }
            
        }


        private void CreateRolesandUsers()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "Admin";
                user.Email = "admin@trashcollector.com";
                string userPWD = "poiuyt";

                var chkUser = UserManager.Create(user, userPWD);

                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }
            }
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Customer"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Customer";
                roleManager.Create(role);
            }


        }
    }
}
