using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppIdentity.Data.Services;

namespace WebAppIdentity.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IRecipeService _recipeService;

        public HomeController(IRecipeService recipeService)
        {
            this._recipeService = recipeService;
        }
        public string Index()
        {
            //var serviceProvider = this.HttpContext.RequestServices;
            //serviceProvider.GetService();
            // return Json(obj);
            return "index action";
        }
    }
}
