using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Recipe
{
    class Recipe:INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Step { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string Youtube { get; set; }
        public string StepDescription { get; set; }
        public BindingList<string> Imagesss { get; set; }
        public string UIHeartColor { get; set; }
        public string UIHeartIcon { get; set; }
        public string WidthFood { get; set; }
        public string Colorrrrrr { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
