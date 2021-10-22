﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppIdentity.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        [Display(Name = "名称")]
        [Required(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [Required(ErrorMessage = "菜谱价格不能为空")]
        public double Price { get; set; }
        public string CreatedById { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }
    }
    public class CreateRecipeDto
    {
        public string Name { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
