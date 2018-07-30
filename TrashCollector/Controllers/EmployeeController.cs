using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            db.Configuration.LazyLoadingEnabled = false;
            var currentDay = DateTime.Now;
            var currentDayString = currentDay.ToString("d");
            var currentUserId = User.Identity.GetUserId();
            var currentUserZip = db.Users.FirstOrDefault(u => u.Id == currentUserId).ZipCode;
            var todaysPickups = db.Pickups.Where(p => p.date == currentDayString && p.Zipcode == currentUserZip).Include(u => u.User);

            return View(todaysPickups);
        }

        public ActionResult WeeklyPickups()
        {
            //var currentDay = DateTime.Now;
            //var currentDayString = currentDay.ToString("d");
            var customerID = db.Roles.FirstOrDefault(r => r.Name == "Customer").Id;
            var DailyUserRoles = db.Roles.SelectMany(u => u.Users.Where(r => r.RoleId == customerID));
            var customersUsers = from userID in DailyUserRoles
                                 join user in db.Users
                                 on userID.UserId equals user.Id
                                 where userID.UserId == user.Id
                                 select user;
            var currentUserId = User.Identity.GetUserId();
            var currentUserZip = db.Users.FirstOrDefault(u => u.Id == currentUserId).ZipCode;
            var myWeeklyPickups = db.Users.Where(u => u.ZipCode == currentUserZip);
            return View(myWeeklyPickups);
        }

        public ActionResult ConfirmPickup(int pickupID)
        {
            var thisPickup = db.Pickups.FirstOrDefault(p => p.Id == pickupID);
            string thisPickupUserID = thisPickup.User.Id;
            thisPickup.Confirmation = "Confirmed";
            var thisUser = db.Users.FirstOrDefault(u => u.Id == thisPickupUserID);
            thisUser.BillTotal += 3.00;
            db.SaveChanges();
            return RedirectToAction("DailyPickups");
        }
    }
}