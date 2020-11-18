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
using Microsoft.Win32;

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
            //

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var appName = System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";
            using (var Key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true))
                Key.SetValue(appName, 99999, RegistryValueKind.DWord);

            string videoID = RecipeData.Video;
            string html =
                @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                <html>
                    <head>
                        <title>YouTubePagesample</title>
                        <meta http-equiv='X-UA-Compatible' content='IE=11'>
                    </head>
                    <iframe width='350' height='205' src='http://www.youtube.com/embed/{0}' frameborder='0' allowfullscreen></iframe>
                    <body>
                    </body>
                </html>;";
            this.webView.NavigateToString(string.Format(html, videoID));
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

        private void PlayVideoBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
