using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebAppIdentity.Data.Services;
using WebAppIdentity.Models;

namespace WebAppIdentity.Pages.Recipes
{
    [Authorize(Policy = "NameHasD")]
    public class EditRecipeModel : PageModel
    {
        private readonly IRecipeService _recipeService;
        private readonly IAuthorizationService _authorizationService;

        public EditRecipeModel(IRecipeService recipeService, IAuthorizationService authorizationService)
        {
            this._recipeService = recipeService;
            this._authorizationService = authorizationService;
        }
        [BindProperty(SupportsGet = true)]
        public Recipe Recipe { get; set; }


        public async Task<IActionResult> OnGet(int recipeId)
        {
            this.Recipe = await this._recipeService.GetRecipeById(recipeId);
            if (this.Recipe == null)
                return NotFound();
            return Page();
           


            //this.Recipe = await this._recipeService.GetRecipeById(recipeId);
            //var authResult = await _authorizationService.AuthorizeAsync(User, Recipe, "IsRecipeOwner");
            //this.CanEditRecipe = authResult.Succeeded;
            ////if (!authResult.Succeeded)
            ////{
            ////    return new ForbidResult();
            ////}
            //return Page();

        }

        public async Task<IActionResult> OnPostAsync(int recipedId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var recipeToUpdate = await this._recipeService.GetRecipeById(recipedId);
            if (recipeToUpdate == null)
                return NotFound();
            try
            {

                //await this._recipeService.EditRecipe(this.Recipe);
                var updateResult = await TryUpdateModelAsync<Recipe>(recipeToUpdate, "Recipe", r => r.Name);
                if (updateResult)
                {
                    await this._recipeService.EditRecipe(recipeToUpdate);
                    return RedirectToPage("/Index");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this._recipeService.RecipeExists(this.Recipe.RecipeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;

                }
            }

            return Page();
        }
    }
}
