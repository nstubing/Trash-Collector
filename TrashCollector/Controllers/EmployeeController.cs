using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrashCollector.Models;

namespace TrashCollector.Controllers
{
    public class EmployeeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Employee
        public ActionResult DailyPickups()
        {
            var currentDay = DateTime.Now;
            var currentDayString = currentDay.ToString("d");
            var customerID = db.Roles.FirstOrDefault(r => r.Name == "Customer").Id;
            var DailyUserRoles = db.Roles.SelectMany(u => u.Users.Where(r => r.RoleId == customerID));
            var customersUsers = from userID in DailyUserRoles
                                 join user in db.Users
                                 on userID.UserId equals user.Id
                                 where userID.UserId == user.Id
                                 select user;
            var dayPickups = customersUsers.Where(u => u.ScheduledDay == currentDay.DayOfWeek.ToString());
            var dayMinusExcluded = dayPickups.Where(u => u.ExcludedStartDate > currentDay || u.ExcludedEndDate < currentDay);
           foreach(ApplicationUser customer in dayMinusExcluded)
            {
                Pickup newPickup = new Pickup();
                newPickup.User = customer;
                newPickup.date =currentDay.ToString("d");
                db.Pickups.Add(newPickup);
            }
            db.SaveChanges();
            var todaysPickups = db.Pickups.Where(p => p.date == currentDayString);

            return View(todaysPickups);
        }
    }
}