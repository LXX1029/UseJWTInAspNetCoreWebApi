using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppIdentity.Data.Services;
using WebAppIdentity.Models;

namespace WebAppIdentity.Pages.Recipes
{
    public class AddRecipeModel : PageModel
    {
        private readonly RecipeService _recipeService;

        public AddRecipeModel(RecipeService recipeService)
        {
            this._recipeService = recipeService;
        }
        public void OnGet()
        {
        }
        [BindProperty]
        public Recipe Recipe { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await this._recipeService.CreateRecipe(Recipe);
            return RedirectToPage("/Index");
        }
    }
}
