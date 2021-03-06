﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrashCollector.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("RoleOptions", "Admin");
            }
            else if (User.IsInRole("Employee"))
            {
                return RedirectToAction("DailyPickups", "Employee");
            }
            else if (User.IsInRole("Customer"))
            {
                return RedirectToAction("Home", "Customer");
            }
            else
            {
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Pricing()
        {
            return View();
        }
        public ActionResult Features()
        {
            return View();
        }
    }
}