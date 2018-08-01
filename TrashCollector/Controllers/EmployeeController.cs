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
            var currentDay = DateTime.Now;
            var currentDayString = currentDay.ToString("d");
            var currentUserId = User.Identity.GetUserId();
            var currentUserZip = db.Users.FirstOrDefault(u => u.Id == currentUserId).ZipCode;
            var todaysPickups = db.Pickups.Where(p => p.date == currentDayString && p.Zipcode == currentUserZip).Include(u => u.User);

            return View(todaysPickups);
        }

        public ActionResult WeeklyPickups()
        {
            var customerID = db.Roles.FirstOrDefault(r => r.Name == "Customer").Id;
            var DailyUserRoles = db.Roles.SelectMany(u => u.Users.Where(r => r.RoleId == customerID));
            var customersUsers = from userID in DailyUserRoles
                                 join user in db.Users
                                 on userID.UserId equals user.Id
                                 where userID.UserId == user.Id
                                 select user;
            var currentUserId = User.Identity.GetUserId();
            var currentUserZip = db.Users.FirstOrDefault(u => u.Id == currentUserId).ZipCode;
            var myWeeklyPickups = customersUsers.Where(u => u.ZipCode == currentUserZip);
            List<string> myDays = new List<string> { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday","All" };
            ViewBag.Days = myDays;
            return View(myWeeklyPickups);
        }

        public ActionResult ConfirmPickup(int id)
        {
            var thisPickup = db.Pickups.FirstOrDefault(p => p.Id == id);
            string thisPickupUserID = thisPickup.UserId;
            thisPickup.Confirmation = "Confirmed";
            var thisUser = db.Users.FirstOrDefault(u => u.Id == thisPickupUserID);
            thisUser.BillTotal += 5.00;
            db.SaveChanges();
            return RedirectToAction("DailyPickups");
        }
        [HttpPost]
        public ActionResult WeeklyPickups(string SortByDate)
        {
            if(SortByDate=="All")
            {
                return RedirectToAction("WeeklyPickups");
            }
            var customerID = db.Roles.FirstOrDefault(r => r.Name == "Customer").Id;
            var DailyUserRoles = db.Roles.SelectMany(u => u.Users.Where(r => r.RoleId == customerID));
            var customersUsers = from userID in DailyUserRoles
                                 join user in db.Users
                                 on userID.UserId equals user.Id
                                 where userID.UserId == user.Id
                                 select user;
            var currentUserId = User.Identity.GetUserId();
            var currentUserZip = db.Users.FirstOrDefault(u => u.Id == currentUserId).ZipCode;
            var myWeeklyPickups = customersUsers.Where(u => u.ZipCode == currentUserZip);
            var OrderedByDate = myWeeklyPickups.Where(u => u.ScheduledDay == SortByDate);
            List<string> myDays = new List<string> { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "All" };
            ViewBag.Days = myDays;
            return View(OrderedByDate);
        }

        public ActionResult Map(int id)
        {
            var thisPickup = db.Pickups.FirstOrDefault(p=>p.Id==id);
            var User = db.Users.FirstOrDefault(u => u.Id == thisPickup.UserId);
            string Address = User.Address +" "+ User.City+" " + User.State+" " + User.ZipCode;
            ViewBag.Info = User.FirstName + " " + User.LastName + " : " + Address;
            ViewBag.Address = Address;
            string key = MyKeys.GOOGlE_API_KEY;
            string myKey = "https://maps.googleapis.com/maps/api/js?key=" + key + "&callback=initMap";
            ViewBag.myKey = myKey;
            return View();

        }

        public ActionResult AllPickups()
        {
            var currentDay = DateTime.Now;
            var currentDayString = currentDay.ToString("d");
            var currentUserId = User.Identity.GetUserId();
            var currentUserZip = db.Users.FirstOrDefault(u => u.Id == currentUserId).ZipCode;
            var todaysPickups = db.Pickups.Where(p => p.date == currentDayString && p.Zipcode == currentUserZip).Include(u => u.User);
            ViewBag.CurrentZip = currentUserZip;
            List<string> myAddresses = new List<string>();
            foreach (Pickup thisPickup in todaysPickups)
            {
                string address = thisPickup.User.Address + " " + thisPickup.User.City + " " + thisPickup.User.State + " " + thisPickup.User.ZipCode;
                myAddresses.Add(address);
            }
            ViewBag.Start = myAddresses.First();
            ViewBag.End = myAddresses.Last();
            var stopsMinusStart = myAddresses.Skip(1);
            var stops = stopsMinusStart.Take(stopsMinusStart.Count() - 1).ToArray();
            ViewBag.Stops = stops;
            ViewBag.Length = stops.Length;
            string key = MyKeys.GOOGlE_API_KEY;
            string myKey = "https://maps.googleapis.com/maps/api/js?key=" + key + "&callback=initMap";
            ViewBag.myKey = myKey;
            return View();
        }
    }
}