using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Food_Recipe
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public ObservableCollection<Recipe> splashRecipes;
        private Random rsg = new Random();
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Setting.SaveSetting("startup", "1");
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.SaveSetting("startup", "0");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("FoodRecipe.xml"))
            {
                return;
            }
            else
            {

            }

            splashRecipes = new ObservableCollection<Recipe>();
            HomeScreen.ReadDatabase(splashRecipes);
            int index = rsg.Next(splashRecipes.Count);
            Recipe recipe = splashRecipes[index];
            this.DataContext = recipe;
        }
    }
}
