using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebAppIdentity.Data.Services;
using WebAppIdentity.Models;

namespace WebAppIdentity.Pages.Recipes
{
    public class EditRecipeModel : PageModel
    {
        private readonly RecipeService _recipeService;

        public EditRecipeModel(RecipeService recipeService)
        {
            this._recipeService = recipeService;
        }
        [BindProperty]
        public Recipe Recipe { get; set; }
        public async Task<IActionResult> OnGet(int recipeId)
        {
            this.Recipe = await this._recipeService.GetRecipeById(recipeId);
            if (this.Recipe == null)
                return NotFound();
            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await this._recipeService.EditRecipe(this.Recipe);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this._recipeService.RecipeExists(this.Recipe.RecipeId))
                {
                    return NotFound();
                }else
                {
                    throw;

                }
            }

            return RedirectToPage("/Index");
        }
    }
}
