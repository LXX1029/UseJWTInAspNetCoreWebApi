using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebAppIdentity.CustomConfigure;
using WebAppIdentity.Data.Services;
using WebAppIdentity.Models;

namespace WebAppIdentity.Pages
{
    //[Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRecipeService _recipeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ILogger<IndexModel> logger, IRecipeService recipeService, UserManager<ApplicationUser> userManager
            , IOptions<WeatherOptions> options)
        {
            _logger = logger;
            this._recipeService = recipeService;
            this._userManager = userManager;
            var opts = options;
        }

        private ICollection<Recipe> _recipeList;

        public ICollection<Recipe> RecipeList
        {
            get { return _recipeList; }
            set { _recipeList = value; }
        }
        /// <summary>
        /// 当前登录用户Id
        /// </summary>
        public string CurrentUserId { get; set; }



        public async void OnGet()
        {
            var user = HttpContext.User.Identity;
            this.CurrentUserId = this._userManager.GetUserId(HttpContext.User);

            RecipeList = (List<Recipe>)await this._recipeService.GetRecipes();
            this._logger.LogInformation($"Loaded {this.RecipeList.Count} recipes");
            using (_logger.BeginScope("Scope value"))
            {
                this._logger.LogWarning(new Exception("自定义Warning异常"), $"Microsoft-Warning");
            }
            using (_logger.BeginScope(new Dictionary<string, object> { { "custom value", 12345 } }))
                this._logger.LogError($"Microsoft-Error");

           
        }

        public async Task<IActionResult> OnGetDelete(int? recipeId)
        {
            if (recipeId == null)
                return NotFound();
            await this._recipeService.DeleteRecipe((int)recipeId);
            RecipeList = (List<Recipe>)await this._recipeService.GetRecipes();
            return RedirectToPage("/Index");
        }

        public IActionResult OnGetEdit(int? recipeId)
        {
            if (recipeId == null) return NotFound();

            return RedirectToPage("/Recipes/EditRecipe", new { recipeId = recipeId });


        }
    }
}
