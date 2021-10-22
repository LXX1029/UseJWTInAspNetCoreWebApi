using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppIdentity.Attributes;
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
        public async Task<IActionResult> HomeIndex()
        {
            //ViewData["HomeContent"] = "Home 页面的内容--ViewData";
            //ViewBag.HomeContent = "Home 页面的内容--ViewBag";
            //ViewBag.Title = "home index  Title";

            //var recipe = new Recipe
            //{
            //    RecipeId = 100,
            //    Name = "Recipe1",
            //    IsVegan = true
            //};
            var list = await _recipeService.GetRecipes();
            return View("HomeIndex", list);
        }



        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [PreventDoublePost]
        public IActionResult CreatePost(Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }


            // TODO 写入数据库
            return RedirectToAction("HomeIndex");
        }


        public async Task<IActionResult> Detail(int recipeId)
        {
            if (recipeId == 0)
            {
                return NotFound();

            }
            var recipe = await this._recipeService.GetRecipeById(recipeId);
            if (recipe == null)
                return NotFound();
            return View(recipe);
        }

        public IActionResult Back()
        {
            return RedirectToAction("HomeIndex");
        }

        //public IActionResult Delete()
        //{
        //    return NoContent();
        //}

        public async Task<IActionResult> Delete(int recipeId)
        {
            if (recipeId == 0)
                return NotFound();
            await this._recipeService.DeleteRecipe(recipeId);
            return RedirectToAction("HomeIndex");

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
