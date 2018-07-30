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


        public ActionResult Home()
        {
            return View();
        }
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

        public ActionResult Employees()
        {
            var employeeID = db.Roles.FirstOrDefault(r => r.Name == "Employee").Id;
            var employeeRoles = db.Roles.SelectMany(u => u.Users.Where(r => r.RoleId == employeeID));
            var employeeUsers = from userID in employeeRoles
                                 join user in db.Users
                                 on userID.UserId equals user.Id
                                 where userID.UserId == user.Id
                                 select user;
            return View(employeeUsers);
        }
    }
}
