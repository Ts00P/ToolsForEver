using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToolsForEver.Models;

namespace ToolsForEver.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        teunstah_toolsforeverEntities db = new teunstah_toolsforeverEntities();
        ApplicationDbContext context;
        ApplicationUserManager _userManager;

        public EmployeeController()
        {
        }

        public EmployeeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            context = new ApplicationDbContext();
            UserManager = userManager;
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
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

        public ActionResult Index()
        {
            IEnumerable<Employee> employees = db.Employees;

            IEnumerable<EmployeeViewModel> model;

            model = employees.Select(x => new EmployeeViewModel
            {
                ID = x.Id,
                Email = x.AspNetUser.Email,
                Firstname = x.Firstname,
                Lastname = x.Lastname,
                Middlename = x.Middlename,
                IsLockedOut = UserManager.IsLockedOut(x.Id)

            });

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var genders = new string[] { "Man", "Vrouw" };
            ViewBag.Genders = new SelectList(genders);
            return View();
        }

        [HttpPost]
        public ActionResult Create(EmployeeViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = UserManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Employee");

                    var employee = new Employee
                    {
                        Id = user.Id,
                        Email = model.Email,
                        Firstname = model.Firstname,
                        Lastname = model.Lastname,
                        Middlename = model.Middlename
                    };

                    db.Employees.Add(employee);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }


                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult LockAccount(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var employee = db.Employees.Find(id);
            var user = UserManager.FindById(employee.Id);
            var isLockedOut = UserManager.IsLockedOut(employee.Id);

            var model = new EmployeeViewModel
            {
                ID = employee.Id,
                Email = employee.AspNetUser.Email,
                Firstname = employee.Firstname,
                Lastname = employee.Lastname,
                Middlename = employee.Middlename,
                IsLockedOut = isLockedOut
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LockAccount(string id, bool userLockoutState)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(id);

                if (userLockoutState == true)
                {
                    user.LockoutEndDateUtc = DateTime.MaxValue;
                    UserManager.Update(user);

                    return RedirectToAction("LockAccount", new { id });
                }

                if (userLockoutState == false)
                {
                    user.LockoutEndDateUtc = null;
                    UserManager.Update(user);

                    return RedirectToAction("LockAccount", new { id });
                }
            }

            return RedirectToAction("LockAccount", new { id });
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var employee = db.Employees.Find(id);
            var user = UserManager.FindById(id);
            var isLockedOut = UserManager.IsLockedOut(id);

            var model = new EmployeeEditModel
            {
                Firstname = employee.Firstname,
                Lastname = employee.Lastname,
                Middlename = employee.Middlename
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeEditModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = db.Employees.Find(model.ID);

                employee.Firstname = model.Firstname;
                employee.Lastname = model.Lastname;
                employee.Middlename = model.Middlename;

                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
