using System;
using System.Collections.Generic;
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
using Syncfusion.Windows.Shared;

namespace Food_Recipe
{
    public partial class DetailRecipe : UserControl
    {
        public Recipe RecipeData { get; set; }
        public int Index { get; set; } = 0;
        public DetailRecipe(Recipe recipe)
        {
            RecipeData = recipe;
            InitializeComponent();
            stepLabel.Content = $"Bước {Index+1}";
            Carousel.DataContext = RecipeData.StepImages[0];
            foodItem.DataContext = RecipeData;
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BeforeBtn_Click(object sender, RoutedEventArgs e)
        {
            if(Index != 0)
            {
                --Index;
            }

            stepLabel.Content = $"Bước {Index + 1}";
            Carousel.DataContext = RecipeData.StepImages[Index];
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if(++Index == RecipeData.StepImages.Count)
            {
                --Index;
            }

            stepLabel.Content = $"Bước {Index + 1}";
            Carousel.DataContext = RecipeData.StepImages[Index];
        }
    }
}
