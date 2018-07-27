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

        [HttpPost]
        public ActionResult Pickup(string PickedDay)
        {
            var currentUser = User.Identity.GetUserId();
            var UserPickup = db.Users.FirstOrDefault(u => u.Id == currentUser);
            UserPickup.ScheduledDay = PickedDay;
            db.SaveChanges();
            return RedirectToAction("Pickup");
        }
    }
}
