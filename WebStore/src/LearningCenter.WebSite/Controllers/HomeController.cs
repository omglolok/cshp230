using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LearningCenter.Business;
using LearningCenter.WebSite.Models;

namespace LearningCenter.WebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryManager categoryManager;
        private readonly IUserManager userManager;
        public HomeController(ICategoryManager categoryManager, IUserManager userManager)
        {
            this.categoryManager = categoryManager;
            this.userManager = userManager;
        }
        public ActionResult Index()
        {
            var categories = categoryManager.Categories
                .Select(t => new LearningCenter.WebSite.Models.CategoryModel(t.Id, t.Name))
                .ToArray();
            var model = new IndexModel { Categories = categories };
            return View(model);
        }

        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(LoginModel loginModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.LogIn(loginModel.Email, loginModel.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Username and password do not match.");
                }
                else
                {
                    Session["User"] = new UserModel { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, Username = user.Username };
                    System.Web.Security.FormsAuthentication.SetAuthCookie(loginModel.Email, false);
                    return Redirect(returnUrl ?? "~/");
                }
            }
            return View(loginModel);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.Register(registerModel.Email, registerModel.Password);
                // when registered, log the user in
                if (user != null)
                {
                    var login = new LoginModel { Id = user.Id, Email = user.Email, Password = registerModel.Password };
                    LogIn(login, null);
                    return Redirect("~/");
                }
                else
                {
                    ModelState.AddModelError("", "This email is already registered with us!");
                }
            }

            return View();
        }
        public ActionResult LogOff()
        {
            Session["User"] = null;
            System.Web.Security.FormsAuthentication.SignOut();
            return Redirect("~/");
        }
        public ActionResult About()
        {
            ViewBag.Message = "The center for learning.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}