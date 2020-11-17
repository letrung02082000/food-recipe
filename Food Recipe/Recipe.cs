using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Food_Recipe
{
    public class Recipe:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        //public string Step { get; set; }
        //public string StepDescription { get; set; }
        public BindingList<StepImages> StepImages { get; set; }
        public string Video { get; set; }
        public bool Favorite { get; set; }
        private string favoriteIcon;
        public string FavoriteIcon
        {
            get => favoriteIcon;
            set
            {
                favoriteIcon = value;
                OnPropertyChanged("FavoriteIcon");
            }
        }

        private string favoriteColor;
        public string FavoriteColor
        {
            get => favoriteColor;
            set
            {
                favoriteColor = value;
                OnPropertyChanged("favoriteColor");
            }
        }

        public Recipe()
        {

        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
                
        }
    }
}
