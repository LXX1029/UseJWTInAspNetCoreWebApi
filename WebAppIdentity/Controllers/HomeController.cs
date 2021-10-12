using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppIdentity.Data.Services;
using WebAppIdentity.Models;

namespace WebAppIdentity.Controllers
{
    //[Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IRecipeService _recipeService;

        public HomeController(IRecipeService recipeService)
        {
            this._recipeService = recipeService;
        }
        //public string Index()
        //{
        //    //var serviceProvider = this.HttpContext.RequestServices;
        //    //serviceProvider.GetService();
        //    // return Json(obj);
        //    return "index action";
        //}
        public IActionResult HomeIndex()
        {
            ViewData["HomeContent"] = "Home 页面的内容--ViewData";
            ViewBag.HomeContent = "Home 页面的内容--ViewBag";
            ViewBag.Title = "home index  Title";

            var recipe = new Recipe
            {
                RecipeId = 100,
                Name = "Recipe1",
                IsVegan = true
            };

            return View("HomeIndex",recipe);
        }
        //[Route("/home/ContainRouteParameters/{id1?}")]
        [Route("~/ContainRouteParameters/{id1?}")]
        public string ContainRouteParameters(int id1)
        {
            return id1.ToString();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
