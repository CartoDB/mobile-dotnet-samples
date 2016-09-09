using CartoMap.WindowsPhone.Pages.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CartoMap.WindowsPhone.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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

            System.Diagnostics.Debug.WriteLine((Window.Current.Content as Frame).CanGoBack + " - " + (Window.Current.Content as Frame).CanGoForward);

            Window.Current.Content = View;

            //(Window.Current.Content as Frame).Navigate(typeof(CartoVisPage));
            //Frame.Content = View;
        }

        private async void OnListItemClick(object sender, SelectionChangedEventArgs e)
        {
            MapListItem item = (MapListItem)e.AddedItems[0];
            System.Diagnostics.Debug.WriteLine(item.GetType());
            System.Diagnostics.Debug.WriteLine("ListView Click: " + e.AddedItems);

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () => Frame.Navigate(item.Type, item.Name)
            );
            Frame.NavigationFailed += NavigationFailed;
            System.Diagnostics.Debug.WriteLine(Frame.CanGoBack + " - " + Frame.CanGoForward);

            //Frame.Navigate(item.Type, item.Name);
            //Frame.Navigate(typeof(MapBasePage), item.Name);
            
            //View.SelectedIndex = -1;
        }

        private void NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Navigation failed: " + e.Exception.Message);
        }
    }
}
