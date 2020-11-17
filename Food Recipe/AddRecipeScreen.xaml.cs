using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Food_Recipe
{
    /// <summary>
    /// Interaction logic for AddRecipeScreen.xaml
    /// </summary>
    public partial class AddRecipeScreen : UserControl
    {
        public string RecipeName { get; set; } = "";
        public string RecipeDescription { get; set; } = "";
        public string CoverImage { get; set; } = "";
        public string VideoID { get; set; } = "";
        public BindingList<string> ImagesList = new BindingList<string>();
        public StepImages Step { get; set; }
        public BindingList<StepImages> StepsList = new BindingList<StepImages>();
        public AddRecipeScreen()
        {
            InitializeComponent();
        }

        private void AddCover_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                CoverImage = filePath;
            }
            else
            {
                MessageBox.Show("Cannot open file.");
            }
        }

        private void AddStepImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                ImagesList.Add(filePath);
            }
            else
            {
                MessageBox.Show("Cannot open file.");
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            RecipeName = nameTextBox.Text.Trim();
            RecipeDescription = descriptionTextBox.Text.Trim();
            VideoID = videoIdTextBox.Text.Trim();

            //Save Cover Image
            if (!Directory.Exists("Images"))
            {
                Directory.CreateDirectory("Images");
            }

            string folder = AppDomain.CurrentDomain.BaseDirectory;
            string coverImageFilePath = $"Images\\{Guid.NewGuid()}.{CoverImage.Split('.').Last()}";
            System.IO.File.Copy(CoverImage, $"{folder}\\{coverImageFilePath}", true);

            //Save Step Images
            Directory.CreateDirectory($"StepImages\\{RecipeName}");
            foreach (StepImages step in StepsList)
            {
                for(int i=0; i < step.StepImagesList.Count; ++i)
                {
                    string stepImageFilePath = $"StepImages\\{RecipeName}\\{Guid.NewGuid()}.{step.StepImagesList[i].Split('.').Last()}";
                    File.Copy(step.StepImagesList[i], $"{folder}\\{stepImageFilePath}", true);
                    step.StepImagesList[i] = stepImageFilePath;
                }
            }
        }

        private void AddStepBtn_Click(object sender, RoutedEventArgs e)
        {
            if (stepDescriptionTextBox.Text != null && ImagesList.Count > 0)
            {
                BindingList<string> newImagesList = new BindingList<string>();
                
                foreach(string image in ImagesList)
                {
                    newImagesList.Add(image);
                }

                Step = new StepImages()
                {
                    StepContent = stepDescriptionTextBox.Text,
                    StepImagesList = newImagesList
                };

                StepsList.Add(Step);
                stepDescriptionTextBox.Clear();
                ImagesList.Clear();
            }
            else
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
            }
        }
    }
}
