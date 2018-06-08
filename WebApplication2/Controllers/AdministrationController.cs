using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApplication2.Models;
using System.Net;

namespace WebApplication2.Controllers
{
    [Authorize(Roles = "Configuration,Admin")]
    public class AdministrationController : Controller
    {
        // GET: Administration
      
        public UserManager<ApplicationUser> UserManager { get; private set; }
        public RoleManager<IdentityRole> RoleManager { get; private set; }
        public ApplicationDbContext context { get; private set; }
        public AdministrationController()
        {
            context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        }

        public AdministrationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        public ActionResult Administration()
        {
            return View();
        }
        public async Task<ActionResult> Index()
        {
            return View(RoleManager.Roles);
        }
        // POST: /Roles/Create

        [HttpPost]

        public ActionResult CreateRole(FormCollection collection)
        {
            try
                {
                    if (ModelState.IsValid)
                    {
                        var role = new IdentityRole(collection["RoleName"]);
                        var roleCheck = new IdentityRole();
                        roleCheck = RoleManager.FindByName(collection["RoleName"]);
                        if (roleCheck==null)
                        {
                            var roleresult = RoleManager.CreateAsync(role);
                        }
                        return RedirectToAction("Administration");
                    }

                   
                    ViewBag.ResultMessage = "Role created successfully !";

                    return RedirectToAction("Administration");

                }
            catch
                {
                    return View();
                }

        }
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Name,Id")] IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await RoleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First().ToString());
                    return View();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //
       

       
    }
}