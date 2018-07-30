using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrashCollector.Models;

namespace TrashCollector.Controllers
{
    public class CustomerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customer
        public ActionResult Pickup()
        {
            var currentUser = User.Identity.GetUserId();
            var UserPickup = db.Users.FirstOrDefault(u => u.Id == currentUser);
            ViewBag.PickupDate = UserPickup.ScheduledDay;
            List<string> Days = new List<string> { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            ViewBag.Days = Days;
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Payment()
        {
            int currentMonth = DateTime.Today.Month;
            var currentYear = DateTime.Today.Year;
            var lastDay = DateTime.DaysInMonth(currentYear, currentMonth);
            string lastDayOfMonth = currentMonth.ToString() + "/" + lastDay.ToString()  +  "/" + currentYear.ToString();
            var currentUserID = User.Identity.GetUserId();
            var CurrentUser = db.Users.FirstOrDefault(u => u.Id == currentUserID);
            ViewBag.Bill = CurrentUser.BillTotal;
            ViewBag.EndMonth = lastDayOfMonth;
            return View();
        }
        public ActionResult OneTime()
        {
            var currentUser = User.Identity.GetUserId();
            var UserPickup = db.Users.FirstOrDefault(u => u.Id == currentUser);
            var currentDay = DateTime.Now;
            var myPickups = db.OneTimePickups.Where(o => o.UserId == UserPickup.Id).ToList();
            ViewBag.OneTimeDays = myPickups;
            return View();
        }
        [HttpPost]
        public ActionResult OneTime(DateTime pickup)
        {
            var currentUser = User.Identity.GetUserId();
            var UserPickup = db.Users.FirstOrDefault(u => u.Id == currentUser);
            OneTimePickup myPickup = new OneTimePickup();
            myPickup.User = UserPickup;
            string newDate = pickup.ToString("d");
            myPickup.date = newDate;
            db.OneTimePickups.Add(myPickup);
            db.SaveChanges();
            return RedirectToAction("OneTime");
        }

        [HttpPost]
        public ActionResult Pickup(string PickedDay)
        {
            var currentUser = User.Identity.GetUserId();
            var UserPickup = db.Users.FirstOrDefault(u => u.Id == currentUser);
            UserPickup.ScheduledDay = PickedDay;
            db.SaveChanges();
            return RedirectToAction("Pickup");
        }
        public ActionResult ExcludedDays()
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.FirstOrDefault(u => u.Id == currentUserId);
            ViewBag.StartDay = currentUser.ExcludedStartDate;
            ViewBag.EndDay = currentUser.ExcludedEndDate;
            return View();
        }
        [HttpPost]
        public ActionResult ExcludedDays(DateTime start, DateTime end)
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.FirstOrDefault(u => u.Id == currentUserId);
            currentUser.ExcludedStartDate = start;
            currentUser.ExcludedEndDate = end;
            db.SaveChanges();
            return RedirectToAction("ExcludedDays");
        }

    }
}
