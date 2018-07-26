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
    public class AdminController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        // GET: Roles

        public ActionResult RoleOptions()
        {
            var list = db.Roles.Select(r=>r.Name);
            ViewBag.Roles = list;
            return View();
        }
        public ActionResult RoleAddToUser(string username)
        {
            ApplicationUser user = db.Users.Where(u => u.UserName == username).FirstOrDefault();
            var account = new AccountController();
            account.UserManager.AddToRole(user.Id, "Employee");
            return RedirectToAction("RoleOptions");
        }
        [HttpPost]
        public ActionResult GetRoles(string username)
        {
            ApplicationUser user = db.Users.Where(u => u.UserName == username).FirstOrDefault();
            var account = new AccountController();
            ViewBag.RolesForThisUser = account.UserManager.GetRoles(user.Id);
            return RedirectToAction("RoleOptions");

        }

        public ActionResult DeleteRoleForUser(string username, string rolename)
        {
            ApplicationUser user = db.Users.Where(u => u.UserName == username).FirstOrDefault();
            var account = new AccountController();
            if (account.UserManager.IsInRole(user.Id, rolename))
            {
                account.UserManager.RemoveFromRole(user.Id, rolename);
                ViewBag.ResultMessage = "Roll Removed";
            }
            else
            {
                ViewBag.ResultMessage = "User not in roll.";
            }

            return RedirectToAction("RoleOptions");
        }
    }
}
