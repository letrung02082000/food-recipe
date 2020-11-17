using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Food_Recipe
{
    /// <summary>
    /// Interaction logic for FavoriteScreen.xaml
    /// </summary>
    public partial class FavoriteScreen : UserControl
    {
        public ObservableCollection<Recipe> recipes;
        public FavoriteScreen()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            recipes = new ObservableCollection<Recipe>();
            Recipe recipe;

            // Read Recipes from XML file
            XDocument xDocument = XDocument.Load("FoodRecipe.xml");
            XElement xElement = xDocument.Root;

            //Read Step
            IEnumerable<XElement> allRecipes =
            from element in xElement.Descendants("Recipe")
            select element;

            foreach (var recipeItem in allRecipes)
            {
                string name = recipeItem.Element("Name").Value;
                string description = recipeItem.Element("Description").Value;
                string image = recipeItem.Element("Image").Value;
                string favoriteString = recipeItem.Element("Favorite").Value;
                int favoriteInt = int.Parse(favoriteString);
                bool favorite = favoriteInt == 1 ? true : false;
                BindingList<StepImages> stepImages = new BindingList<StepImages>();

                IEnumerable<XElement> allSteps =
                from element in recipeItem.Descendants("Step")
                select element;

                foreach (var stepItem in allSteps)
                {
                    string stepContent = stepItem.Element("StepContent").Value;

                    IEnumerable<XElement> allStepImages =
                    from element in stepItem.Descendants("StepImages")
                    select element;

                    BindingList<string> stepImagesList = new BindingList<string>();

                    foreach (var stepImage in allStepImages)
                    {
                        IEnumerable<XElement> allStepImagesItems =
                        from element in stepImage.Descendants("StepImage")
                        select element;



                        foreach (var stepImagesItem in allStepImagesItems)
                        {
                            stepImagesList.Add(stepImagesItem.Value);
                        }
                    }

                    var stepImageInstance = new StepImages()
                    {
                        StepContent = stepContent,
                        StepImagesList = stepImagesList

                    };

                    stepImages.Add(stepImageInstance);
                }
                recipe = new Recipe()
                {
                    Name = name,
                    Description = description,
                    Image = image,
                    StepImages = stepImages,
                    Favorite = favorite,
                };

                if (favorite)
                {
                    recipe.Favorite = true;
                    recipe.FavoriteColor = "red";
                    recipe.FavoriteIcon = "Favorite";
                }
                else
                {
                    recipe.Favorite = false;
                    recipe.FavoriteColor = "AliceBlue";
                    recipe.FavoriteIcon = "FavoriteBorder";
                }

                if(favorite)
                {
                    recipes.Add(recipe);

                }
                
            }

            favoriteDataListView.ItemsSource = recipes;
        }

        private void favoriteDataListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Recipe selectedRecipe = (sender as ListView).SelectedItem as Recipe;

            if (selectedRecipe != null)
            {
                var detailScreen = new DetailRecipe(selectedRecipe);
                RecipeContent.Children.Add(detailScreen);
            }
        }

        private void Favorite_Click(object sender, RoutedEventArgs e)
        {
            XDocument xDocument = XDocument.Load("FoodRecipe.xml");
            XElement xElement = xDocument.Root;

            IEnumerable<XElement> allFavoriteRecipes =
            from element in xElement.Descendants("Recipe")
            where element.Element("Favorite").Value == "1"
            select element;

            var index = favoriteDataListView.Items.IndexOf((sender as FrameworkElement).DataContext);

            if (recipes[index].Favorite)
            {
                recipes[index].Favorite = false;
                recipes[index].FavoriteColor = "AliceBlue";
                recipes[index].FavoriteIcon = "FavoriteBorder";
                recipes.RemoveAt(index);

                //Store data to XML database
                allFavoriteRecipes.ElementAt(index).Element("Favorite").Value = "0";
                xDocument.Save("FoodRecipe.xml");
            }
            else
            {
                recipes[index].Favorite = true;
                recipes[index].FavoriteColor = "red";
                recipes[index].FavoriteIcon = "Favorite";
                recipes.RemoveAt(index);

                //Store data to XML database
                allFavoriteRecipes.ElementAt(index).Element("Favorite").Value = "1";
                xDocument.Save("FoodRecipe.xml");
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
