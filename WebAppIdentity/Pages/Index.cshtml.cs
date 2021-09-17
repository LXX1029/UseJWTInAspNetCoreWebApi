using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppIdentity.Data.Services;
using WebAppIdentity.Models;

namespace WebAppIdentity.Pages
{
    //[Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly RecipeService _recipeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ILogger<IndexModel> logger, RecipeService recipeService, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this._recipeService = recipeService;
            this._userManager = userManager;
        }

        private List<Recipe> _recipeList;

        public List<Recipe> RecipeList
        {
            get { return _recipeList; }
            set { _recipeList = value; }
        }
        public string CurrentUserId { get; set; }



        public async void OnGet()
        {
            var user = HttpContext.User.Identity;
            this.CurrentUserId = this._userManager.GetUserId(HttpContext.User);

            RecipeList = (List<Recipe>)await this._recipeService.GetRecipes();


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
