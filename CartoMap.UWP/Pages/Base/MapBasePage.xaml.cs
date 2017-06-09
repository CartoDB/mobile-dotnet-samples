using Carto.Ui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CartoMap.WindowsPhone.Pages.Map
{
    public partial class MapBasePage : Page
    {
        protected MapView MapView { get; set; }

        public MapBasePage()
        {
            InitializeComponent();

            MapView = new MapView();

            Window.Current.Content = MapView;
        }
    }
}
