using System;
using System.Text.RegularExpressions;
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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Food_Recipe
{
    /// <summary>
    /// Interaction logic for HomeScreen.xaml
    /// </summary>
    public partial class HomeScreen : UserControl
    {
        public int PageItem { get; set; }
        public bool OnSearch { get; set; } = false;
        public ObservableCollection<Recipe> searchRecipes = new ObservableCollection<Recipe>();
        public HomeScreen()
        {
            InitializeComponent();
            //foodDataListView.DataContext = this;
            SettingScreen settingScreen = new SettingScreen();
            settingScreen.PageItemChanged += SettingScreen_pageItemChanged;
        }

        private void SettingScreen_pageItemChanged(int newPageItem)
        {
            PageItem = newPageItem;
        }

        public ObservableCollection<Recipe> recipes;
        //public event PropertyChangedEventHandler PropertyChanged;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XDocument xDocument = XDocument.Load("FoodRecipe.xml");
            XElement xElement = xDocument.Root;

            

            if (OnSearch)
            {
                int searchIndex = foodDataListView.Items.IndexOf((sender as FrameworkElement).DataContext);
                Recipe searchRecipe = ((sender as FrameworkElement).DataContext) as Recipe;
                string searchRecipeName = searchRecipe.Name;

                IEnumerable<XElement> allSearch = xElement.Descendants("Recipe")
                    .Where(element =>
                    {
                        string value = element.Element("Name").Value;
                        return value == searchRecipeName;
                    });
                
                foreach(var searchItem in allSearch)
                {
                    bool favoriteStatus = int.Parse(searchItem.Element("Favorite").Value) == 1 ? true : false;
                    if (favoriteStatus)
                    {
                        searchItem.Element("Favorite").Value = "0";
                        searchRecipes[searchIndex].Favorite = false;
                        searchRecipes[searchIndex].FavoriteColor = "AliceBlue";
                        searchRecipes[searchIndex].FavoriteIcon = "FavoriteBorder";
                    }
                    else
                    {
                        searchItem.Element("Favorite").Value = "1";
                        searchRecipes[searchIndex].Favorite = false;
                        searchRecipes[searchIndex].FavoriteColor = "red";
                        searchRecipes[searchIndex].FavoriteIcon = "Favorite";
                    }
                }
                xDocument.Save("FoodRecipe.xml");
                return;
            }


            IEnumerable<XElement> allRecipes =
            from element in xElement.Descendants("Recipe")
            select element;

            var index = Setting.PageItem * Setting.PageCurrent + foodDataListView.Items.IndexOf((sender as FrameworkElement).DataContext);

            if (recipes[index].Favorite)
            {
                recipes[index].Favorite = false;
                recipes[index].FavoriteColor = "AliceBlue";
                recipes[index].FavoriteIcon = "FavoriteBorder";

                //Store data to XML database
                allRecipes.ElementAt(index).Element("Favorite").Value = "0";
                xDocument.Save("FoodRecipe.xml");
            }
            else
            {
                recipes[index].Favorite = true;
                recipes[index].FavoriteColor = "red";
                recipes[index].FavoriteIcon = "Favorite";

                //Store data to XML database
                allRecipes.ElementAt(index).Element("Favorite").Value = "1";
                xDocument.Save("FoodRecipe.xml");
            }
        }

        Setting setting = new Setting();
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            recipes = new ObservableCollection<Recipe>();

            ReadDatabase(recipes);

            int itemNumber = recipes.Count;
            int pageItem = Setting.PageItem;
            Setting.PageNumber = (itemNumber / pageItem) + (itemNumber % pageItem == 0 ? 0 : 1);
            Setting.PageCurrent = 0;
            NotificationText.Text = $"{Setting.PageCurrent + 1} trên {Setting.PageNumber}";
            foodDataListView.ItemsSource = recipes.Take(Setting.PageItem);
        }

        

        private void foodDataListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Recipe selectedRecipe = (sender as ListView).SelectedItem as Recipe;

            if (selectedRecipe != null)
            {
                var detailScreen = new DetailRecipe(selectedRecipe);
                RecipeContent.Children.Add(detailScreen);
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            OnSearch = false;
            if (Setting.PageCurrent == (Setting.PageNumber-1)) return;
            Setting.PageCurrent++;
            NotificationText.Text = $"{Setting.PageCurrent + 1} trên {Setting.PageNumber}";
            foodDataListView.ItemsSource = recipes.Skip(Setting.PageItem * Setting.PageCurrent).Take(Setting.PageItem);
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            OnSearch = false;
            if (Setting.PageCurrent == 0) return;
            Setting.PageCurrent--;
            NotificationText.Text = $"{Setting.PageCurrent + 1} trên {Setting.PageNumber}";
            foodDataListView.ItemsSource = recipes.Skip(Setting.PageItem * Setting.PageCurrent).Take(Setting.PageItem);
        }

        private void Search(string textBoxValue)
        {
            searchRecipes.Clear();
            OnSearch = true;
            textBoxValue = ConvertToUnSign(textBoxValue).ToLower();

            if (textBoxValue == "")
            {
                OnSearch = false;
                ReadDatabase(recipes);
                foodDataListView.ItemsSource = recipes.Skip(Setting.PageCurrent * Setting.PageItem).Take(Setting.PageItem);
                NotificationText.Text = $"{Setting.PageCurrent + 1} trên {Setting.PageNumber}";
                return;
            }

            NotificationText.Text = "Kết quả tìm kiếm của bạn";
            XDocument xDocument = XDocument.Load("FoodRecipe.xml");
            XElement doc = xDocument.Root;
            var allSearchRecipes = doc.Descendants("Recipe")
                .Where(
                    element =>
                    {
                        string value = ConvertToUnSign(element.Element("Name").Value).ToLower();
                        return value.Contains(textBoxValue);
                    }

                ).ToList();

            Recipe recipe;

            foreach (var recipeItem in allSearchRecipes)
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
                    StepImages = stepImages
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

                searchRecipes.Add(recipe);
            }

            foodDataListView.ItemsSource = searchRecipes;

            if(searchRecipes.Count == 0)
            {
                NotificationText.Text = "Không tìm thấy món ăn bạn muốn. Hãy thêm món ăn";
            }
        }

    
        private string ConvertToUnSign(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string textBoxValue = (sender as TextBox).Text;
            Search(textBoxValue);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            string textBoxValue = (sender as TextBox).Text;
            if (e.Key == Key.Return)
            {
                Search(textBoxValue);
                
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            
            string textBoxValue = SearchTextBox.Text;
            Search(textBoxValue);
        }

        public static void ReadDatabase(ObservableCollection<Recipe> DBrecipes)
        {
            DBrecipes.Clear();

            Recipe recipe;
            // Read Recipes from XML file
            XDocument xDocument = XDocument.Load("FoodRecipe.xml");
            XElement xElement = xDocument.Root;

            //Read Step
            IEnumerable<XElement> allRecipes =
            from element in xElement.Descendants("Recipe")
            select element;

            //List<XElement> allRecipesList = allRecipes.ToList();

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
                    StepImages = stepImages
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

                DBrecipes.Add(recipe);
            }
        }
    }
}
