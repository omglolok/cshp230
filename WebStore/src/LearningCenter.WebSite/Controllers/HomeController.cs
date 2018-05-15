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
        private readonly IClassManager classManager;
        private readonly ICartManager cartManager;
        private static int userId = -1;
        public HomeController(ICategoryManager categoryManager,
                              IUserManager userManager,
                              IClassManager classManager,
                              ICartManager cartManager)
        {
            this.categoryManager = categoryManager;
            this.userManager = userManager;
            this.classManager = classManager;
            this.cartManager = cartManager;
        }
        public ActionResult Index()
        {

            return View();
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
                    userId = user.Id;
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

        public ActionResult OurClasses()
        {
            var categories = categoryManager.Categories
                .Select(t => new Models.CategoryModel(t.Id, t.Name))
                .ToArray();
            var model = new IndexModel();
            var categoryList = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                var classes = classManager
                                    .ClassesByCategory(category.Id)
                                    .Select(t =>
                                        new Models.ClassModel
                                        {
                                            Id = t.Id,
                                            Name = t.Name,
                                            Description = t.Description,
                                            Price = t.Price,
                                            Sessions = t.Sessions
                                        }).ToArray();

                categoryList.Add(new CategoryViewModel { Category = new Models.CategoryModel(category.Id, category.Name), Classes = classes });

            }
            model.Categories = categoryList;
            return View(model);
        }

        public ActionResult YourClasses()
        {
            var categories = categoryManager.Categories
                .Select(t => new Models.CategoryModel(t.Id, t.Name))
                .ToArray();
            var model = new IndexModel();
            var categoryList = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                if (userId != -1)
                {
                    var classes = classManager
                                        .ClassesByUser(userId)
                                        .Select(t =>
                                            new Models.ClassModel
                                            {
                                                Id = t.Id,
                                                Name = t.Name,
                                                Description = t.Description,
                                                Price = t.Price,
                                                Sessions = t.Sessions
                                            }).ToArray();

                    categoryList.Add(new CategoryViewModel { Category = new Models.CategoryModel(category.Id, category.Name), Classes = classes });

                }
                model.Categories = categoryList;
                return View(model);
            }
            return View();
        }
        public ActionResult ViewCart()
        {
            var cart = cartManager.GetAllClasses(userId)
                .Select(t => new CartItem
                {
                    Id = t.Id,
                    Name = t.Name,
                    Price = t.Price
                })
                .ToArray();

            return View(cart);
        }
        public ActionResult AddToCart(int classId)
        {
            if (userId != -1)
            {
                cartManager.Add(userId, classId);
            }
            return Redirect("~/Home/ViewCart");
        }
        public ActionResult Enroll(int classId)
        {
            if (userId != -1)
            {
                cartManager.Enroll(userId, classId);
                cartManager.Remove(userId, classId);
            }
            return Redirect("~/Home/YourClasses");
        }
        public ActionResult Remove(int classId)
        {
            if (userId != -1)
            {
                cartManager.Remove(userId, classId);
            }
            return Redirect("~/Home/ViewCart");
        }
    }
}