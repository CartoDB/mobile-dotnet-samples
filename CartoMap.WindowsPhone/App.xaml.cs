using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CartoMap.WindowsPhone
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        /// Invoked when the application is launched normally by the end user.  Other entry points
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            ListBox view = new ListBox();
            view.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 240, 240, 240));
            view.Padding = new Thickness(0, 20, 0, 0);
            view.ItemsSource = Samples.List;
            view.ItemTemplate = (DataTemplate)Resources["ListItemTemplate"];

            new DataTemplate { };

            Window.Current.Content = view;

            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }

    public class ListItemTemplate : DataTemplate
    {
        public TextBlock Name { get; set; }

        public TextBlock Description { get; set; }

        public ListItemTemplate()
        {
            Name = new TextBlock { Height = 50, Width = 100};
            Description = new TextBlock { Height = 50, Width = 100 };

            Name.Text = "ASDF";
            Description.Text = "Description";
        }
    }
}
