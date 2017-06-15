using CartoMap.WindowsPhone.Pages.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CartoMap.WindowsPhone.Pages
{
    public partial class LauncherListPage : Page
    {
        public ListBox View { get; set; }

        public LauncherListPage()
        {
            InitializeComponent();

            View = new ListBox();
            View.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 240, 240, 240));
            View.Padding = new Thickness(0, 20, 0, 0);
            View.ItemsSource = Samples.List;
            View.ItemTemplate = (DataTemplate)Resources["ListItemTemplate"];
            View.SelectionChanged += OnListItemClick;

            Window.Current.Content = View;
        }

        private async void OnListItemClick(object sender, SelectionChangedEventArgs e)
        {
            MapListItem item = (MapListItem)e.AddedItems[0];

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () => Frame.Navigate(item.Type, item.Name)
            );
        }
    }
}
