using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Food_Recipe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ChangeMenuPointer(int index)
        {
            TrainsitionigContentCursor.OnApplyTemplate();
            MenuPointer.Margin = new Thickness(0, 100 + 60 * index, 0, 0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string pageItem = "5";
            Setting.readSettingDB("pageitem", ref pageItem);
            Setting.PageItem = int.Parse(pageItem);

            GridContent.Children.Clear();
            HomeScreen homeScreen = new HomeScreen();
            GridContent.Children.Add(homeScreen);

        }

        

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            
            ListViewMenu.SelectedIndex = 0;
            ChangeMenuPointer(0);
            GridContent.Children.Clear();
            HomeScreen homeScreen = new HomeScreen();
            GridContent.Children.Add(homeScreen);
        }



        private void Favorite_Click(object sender, RoutedEventArgs e)
        {
            ListViewMenu.SelectedIndex = 1;
            ChangeMenuPointer(1);
            GridContent.Children.Clear();
            FavoriteScreen favoriteScreen = new FavoriteScreen();
            GridContent.Children.Add(favoriteScreen);
        }

        private void AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            ListViewMenu.SelectedIndex = 2;
            ChangeMenuPointer(2);
            GridContent.Children.Clear();
            AddRecipeScreen addRecipeScreen = new AddRecipeScreen();
            GridContent.Children.Add(addRecipeScreen);
        }

        private void Todo_Click(object sender, RoutedEventArgs e)
        {
            ListViewMenu.SelectedIndex = 3;
            ChangeMenuPointer(3);
            GridContent.Children.Clear();
            TodoScreen todoScreen = new TodoScreen();
            GridContent.Children.Add(todoScreen);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            ListViewMenu.SelectedIndex = 4;
            ChangeMenuPointer(4);
            GridContent.Children.Clear();
            AboutScreen aboutScreen = new AboutScreen();
            GridContent.Children.Add(aboutScreen);
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            ListViewMenu.SelectedIndex = 5;
            ChangeMenuPointer(5);
            SettingScreen settingScreen = new SettingScreen();
            GridContent.Children.Clear();
            GridContent.Children.Add(settingScreen);
        }
    }
}
