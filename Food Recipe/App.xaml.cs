using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace Food_Recipe
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string startupWindow = "1";
            bool openStartupWindow;
            Setting.readSettingDB("startup", ref startupWindow);

            if (!File.Exists("FoodRecipe.xml"))
            {
                MessageBox.Show("Không có dữ liệu món ăn. Vui lòng không chỉnh sửa các file hệ thống. Bạn có thể tạo mới các công thức.");
            }
            
            if (startupWindow!=null)
            {
                openStartupWindow = startupWindow == "1" ? true : false;

                if (openStartupWindow)
                {
                    SplashScreen splashScreen = new SplashScreen();
                    splashScreen.Show();
                }
                else
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
            }
            else
            {
                SplashScreen splashScreen = new SplashScreen();
                splashScreen.Show();
            }
        }
    }
}
