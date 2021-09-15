using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppIdentity.Models;

namespace WebAppIdentity.Data.Services
{
    public class RecipeService
    {
        private readonly ApplicationDbContext _context;

        public RecipeService(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<int> CreateRecipe(Recipe recipe)
        {
            this._context.Recipes.Add(recipe);
            await this._context.SaveChangesAsync();
            return recipe.RecipeId;
        }

        public async Task<ICollection<Recipe>> GetRecipes()
        {
            return await this._context.Recipes.ToListAsync();
        }

        public async Task DeleteRecipe(int recipeId)
        {
            var _recipe = await this._context.Recipes.FindAsync(recipeId);
            if (_recipe != null)
            {
                this._context.Recipes.Remove(_recipe);
                await this._context.SaveChangesAsync();
            }
        }

        public async Task<Recipe> GetRecipeById(int recipeId)
        {
            return await this._context.Recipes.FindAsync(recipeId);
        }

        public async Task EditRecipe(Recipe recipe)
        {
            this._context.Entry<Recipe>(recipe).State = EntityState.Modified;
            await this._context.SaveChangesAsync();
        }

        public bool RecipeExists(int recipeId)
        {
            return this._context.Recipes.Any(m => m.RecipeId == recipeId);
        }
    }
}
