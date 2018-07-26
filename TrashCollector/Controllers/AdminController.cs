using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
        public ApplicationUserManager _userManager;        // GET: Roles

        public ActionResult RoleOptions()
        {
            UserManager = UserManager;
            ViewBag.Roles = db.Roles.Select(r => r.Name);
            return View();
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult RoleAddToUser(string username,string rolename)
        { 
            ApplicationUser user = db.Users.Where(u => u.UserName == username).FirstOrDefault();
            var currentRoles = db.Roles.SelectMany(r => r.Users);
            var currentRole = currentRoles.FirstOrDefault(u => u.UserId == user.Id);
            var oldRoleID = db.Roles.FirstOrDefault(r => r.Id == currentRole.RoleId);
            string oldName = oldRoleID.Name;
            var newRoleID = db.Roles.FirstOrDefault(r=>r.Name==rolename);
            UserManager.RemoveFromRole(user.Id, oldName);
            UserManager.AddToRole(user.Id, rolename);
            db.SaveChanges();
            return RedirectToAction("RoleOptions");
        }
        [HttpPost]
        public ActionResult GetRoles(string username)
        {
            ApplicationUser user = db.Users.Where(u => u.UserName == username).FirstOrDefault();
            var currentRoles = db.Roles.SelectMany(r => r.Users);
            var thisUserRole = currentRoles.FirstOrDefault(u => u.UserId == user.Id);
            var thisRoleName = db.Roles.Where(r => r.Id == thisUserRole.RoleId).FirstOrDefault().Name;
            ViewBag.Role = thisRoleName.ToString();
            ViewBag.Roles = db.Roles.Select(r => r.Name);
            return View("RoleOptions");

        }

        //public ActionResult DeleteRoleForUser(string username, string rolename)
        //{
        //    ApplicationUser user = db.Users.Where(u => u.UserName == username).FirstOrDefault();
        //    var account = new AccountController();
        //    if (account.UserManager.IsInRole(user.Id, rolename))
        //    {
        //        account.UserManager.RemoveFromRole(user.Id, rolename);
        //        ViewBag.ResultMessage = "Roll Removed";
        //    }
        //    else
        //    {
        //        ViewBag.ResultMessage = "User not in roll.";
        //    }

        //    return RedirectToAction("RoleOptions");
        //}
    }
}
