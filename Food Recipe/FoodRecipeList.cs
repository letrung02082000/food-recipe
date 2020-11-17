using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Recipe
{
    class FoodRecipeList
    {
        public BindingList<Recipe> RecipeList { get; set; }
        public FoodRecipeList()
        {
            RecipeList = new BindingList<Recipe>();
        }
    }
}
