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
        public HomeController(ICategoryManager categoryManager)
        {
            this.categoryManager = categoryManager;
        }
        public ActionResult Index()
        {
            var categories = categoryManager.Categories
                .Select(t => new LearningCenter.WebSite.Models.CategoryModel(t.Id, t.Name))
                .ToArray();
            var model = new IndexModel { Categories = categories };
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Ground zero for learning programming.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}