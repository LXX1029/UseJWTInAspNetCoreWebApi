using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppIdentity.Data.Services;
using WebAppIdentity.Models;

namespace WebAppIdentity.Pages.Recipes
{
    public class AddRecipeModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RecipeService _recipeService;

        public AddRecipeModel(RecipeService recipeService, UserManager<ApplicationUser> userManager)
        {
            this._recipeService = recipeService;
            this._userManager = userManager;
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
            var appUser = await this._userManager.GetUserAsync(this.User);
            this.Recipe.CreatedById = appUser.Id;
            await this._recipeService.CreateRecipe(Recipe);
            return RedirectToPage("/Index");
        }
    }
}
