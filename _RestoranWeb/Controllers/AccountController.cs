using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using _RestoranWeb.Identity;
using _RestoranWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using _RestoranWeb.Controllers;

namespace _RestoranWeb.Controllers
{
    public class AccountController : Controller
    {

        private UserManager<ApplicationUser> userManager;
        private RoleManager<ApplicationRole> roleManager;

        private Entities dm = new Entities();

        public AccountController()
        {
            IdentityContext IdentityContext = new IdentityContext();
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(IdentityContext);
            userManager = new UserManager<ApplicationUser>(userStore);
            RoleStore<ApplicationRole> roleStore = new RoleStore<ApplicationRole>(IdentityContext);
            roleManager = new RoleManager<ApplicationRole>(roleStore);


        }

        // GET: Account
        public ActionResult Index()
        {

            ViewBag.Semtara = new SelectList( dm.Semt, "SemtId", "SemtAdi");

            //BigViewModel b = new BigViewModel();
            //b.RestoranModel = dm.Restoran.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(BigViewModel model, string searchstring, int? semtara)
        {

            ViewBag.Semtara = new SelectList(dm.Semt, "SemtId", "SemtAdi");
            // Restoran Arama Bölümü
            var restoranlar = from m in dm.Restoran
                              select m;

            if (!string.IsNullOrEmpty(searchstring))
            {
                restoranlar = dm.Semt.Where(p => p.SemtId == semtara).SelectMany(p => p.Restoran).Where(p => p.RestoranAdi.Contains(searchstring));
                return View(restoranlar);
            }
            
            if (ModelState.IsValid)
            {
                ApplicationUser user = userManager.Find(model.LoginModel.Username, model.LoginModel.Password);
                if (user != null)
                {

                    IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
                    ClaimsIdentity identity = userManager.CreateIdentity(user, "ApplicationCookie");
                    AuthenticationProperties authProps = new AuthenticationProperties();
                    authManager.SignIn(authProps, identity);


                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("LoginUser", "Böyle bir kullanıcı bulunamadı.");
                }

            }

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(BigViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.Name = model.RegisterModel.Name;
                user.Surname = model.RegisterModel.Surname;
                user.UserName = model.RegisterModel.Username;
                IdentityResult iResult = userManager.Create(user, model.RegisterModel.Password);
                if (iResult.Succeeded)
                {
                    userManager.AddToRole(user.Id, "User");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("RegisterUser", "Kullanıcı ekleme işleminde hata!");
                }
            }
            return View(model);
        }

        public ActionResult Login()
        {
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(BigViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = userManager.Find(model.LoginModel.Username , model.LoginModel.Password);
                if (user != null)
                {
                    // HttpContext.GetOwinContext().Authentication;
                    IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
                    ClaimsIdentity identity = userManager.CreateIdentity(user, "ApplicationCookie");
                    AuthenticationProperties authProps = new AuthenticationProperties();
                    authManager.SignIn(authProps, identity);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("LoginUser", "Böyle bir kullanıcı bulunamadı");
                }
            }
            
            return RedirectToAction("Index","Home");
        }

        public ActionResult Membership()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Membership(BigViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.Name = model.RegisterModel.Name;
                user.Surname = model.RegisterModel.Surname;
                user.UserName = model.RegisterModel.Username;
                IdentityResult iResult = userManager.Create(user, model.RegisterModel.Password);
                if (iResult.Succeeded)
                {
                    userManager.AddToRole(user.Id, "User");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("RegisterUser", "Kullanıcı ekleme işleminde hata!");
                }
            }
            return View(model);



           
        }

        [Authorize]
        public ActionResult Logout()
        {
            IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Index", "Home");


        }
    }
}