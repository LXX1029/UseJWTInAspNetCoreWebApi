using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppIdentity.Data.Services;
using WebAppIdentity.Models;

namespace WebAppIdentity.Pages.Recipes
{
    public class DetailModel : PageModel
    {
        private readonly RecipeService _recipeService;
        private readonly IAuthorizationService _authorizationService;
        public DetailModel(RecipeService recipeService, IAuthorizationService authorizationService)
        {
            this._recipeService = recipeService;
            this._authorizationService = authorizationService;
        }
        [BindProperty]
        public Recipe Recipe { get; set; }
        public bool CanEditRecipe { get; set; }
        public async Task<IActionResult> OnGet(int recipeId)
        {
            this.Recipe = await this._recipeService.GetRecipeById(recipeId);
            if (this.Recipe == null) return NotFound();
            var authResult = await _authorizationService.AuthorizeAsync(User, Recipe, "IsRecipeOwner");
            this.CanEditRecipe = authResult.Succeeded;
            //if (!authResult.Succeeded)
            //{
            //    return new ForbidResult();
            //}
           
            return Page();
        }
    }
}
