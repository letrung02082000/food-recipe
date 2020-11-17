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
using System.Xml.Linq;

namespace Food_Recipe
{
    /// <summary>
    /// Interaction logic for SettingScreen.xaml
    /// </summary>
    public partial class SettingScreen : UserControl
    {
        public delegate void pageItemChangedHandler(int newPageItem);
        public event pageItemChangedHandler PageItemChanged;
        public SettingScreen()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PageItemButton_Click(object sender, RoutedEventArgs e)
        {
            int PageItem;
            if(int.TryParse(PageItemTextBox.Text, out PageItem))
            {
                if(PageItem > 0)
                {
                    Setting.PageItem = PageItem;
                    Setting.SaveSetting("pageitem", PageItem.ToString());
                    MessageBox.Show("Đã lưu");
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập giá trị lớn hơn 0");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập giá trị là 1 số");
            }
            
        }

        private void SplashSetting_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.SaveSetting("startup", "0");
        }

        private void SplashSetting_Checked(object sender, RoutedEventArgs e)
        {
            Setting.SaveSetting("startup", "1");
        }

        private void SettingScreen_Loaded(object sender, RoutedEventArgs e)
        {
            string splashSetting = "1";
            Setting.readSettingDB("startup", ref splashSetting);
            bool openSplashSetting = splashSetting == "1" ? true : false;
            SplashSetting.IsChecked = openSplashSetting;
        }
    }
}
