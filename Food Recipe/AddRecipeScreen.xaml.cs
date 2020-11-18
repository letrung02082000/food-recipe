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
using System.Xml.Linq;
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
                coverImage.Source = new BitmapImage(new Uri(filePath, UriKind.Absolute));
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

            if(RecipeName == "" || RecipeDescription == "" || CoverImage == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các trường Tên, Mô tả, Video ID");
                return;
            }

            try
            {
                VideoID = videoIdTextBox.Text.Trim().Split('=')[1];
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập đường dẫn video Youtube hợp lệ");
                return;
            }

            if (StepsList.Count == 0)
            {
                MessageBox.Show("Vui lòng điền đầy đủ các trường Mô tả bước, thêm hình ảnh và nhấn Thêm bước");
                return;
            }

            //Save Cover Image
            if (!Directory.Exists("Images"))
            {
                Directory.CreateDirectory("Images");
            }

            string folder = AppDomain.CurrentDomain.BaseDirectory;
            string coverImageFilePath = $"Images\\{Guid.NewGuid()}.{CoverImage.Split('.').Last()}";
            XElement imageElement = new XElement("Image", coverImageFilePath);
            System.IO.File.Copy(CoverImage, $"{folder}\\{coverImageFilePath}", true);

            //Save Step Images
            XElement stepsElement = new XElement("Steps");
            Directory.CreateDirectory($"StepImages\\{RecipeName}");
            foreach (StepImages step in StepsList)
            {
                XElement stepElement = new XElement("Step");
                XElement stepContent = new XElement("StepContent", step.StepContent);
                stepElement.Add(stepContent);
                XElement stepImages = new XElement("StepImages");

                for (int i=0; i < step.StepImagesList.Count; ++i)
                {
                    string stepImageFilePath = $"StepImages\\{RecipeName}\\{Guid.NewGuid()}.{step.StepImagesList[i].Split('.').Last()}";
                    File.Copy(step.StepImagesList[i], $"{folder}\\{stepImageFilePath}", true);
                    step.StepImagesList[i] = stepImageFilePath;
                    //Save stepImages to XML database
                    XElement stepImage = new XElement("StepImage", stepImageFilePath);
                    stepImages.Add(stepImage);
                }
                stepElement.Add(stepImages);
                stepsElement.Add(stepElement);
            }

            XElement nameElement = new XElement("Name", RecipeName);
            XElement descriptionElement = new XElement("Description", RecipeDescription);
            XElement videoIDElement = new XElement("VideoID", VideoID);
            XElement favoriteElement = new XElement("Favorite", "0");

            XElement recipeElement = new XElement("Recipe", nameElement, descriptionElement, imageElement, videoIDElement, favoriteElement, stepsElement);

            //Save to XML database
            if (File.Exists("FoodRecipe.xml"))
            {
                XDocument xmlDoc = XDocument.Load("FoodRecipe.xml");
                XElement recipesElement = xmlDoc.Root;
                
                recipesElement.Add(recipeElement);
                xmlDoc.Save("FoodRecipe.xml");
            }
            else
            {
                XDocument xmlDoc = new XDocument(new XElement("Recipes", recipeElement));
                xmlDoc.Save("FoodRecipe.xml");
            }
            MessageBox.Show("Lưu món ăn thành công!");
            nameTextBox.Clear();
            descriptionTextBox.Clear();
            videoIdTextBox.Clear();
            coverImage.Source = null;
            StepsList.Clear();

        }

        private void AddStepBtn_Click(object sender, RoutedEventArgs e)
        {
            if (stepDescriptionTextBox.Text != "" && ImagesList.Count > 0)
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
                MessageBox.Show("Vui lòng điền đầy đủ các trường Mô tả bước và thêm hình ảnh của bước rồi thử lại.");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            stepImagesItemsControl.ItemsSource = ImagesList;
            stepsListItemsControl.ItemsSource = StepsList;
        }
    }
}
