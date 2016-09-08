
using Carto.Core;
using Carto.Layers;
using Carto.Projections;
using Carto.Ui;

using System;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace HelloMap.WindowsPhone
{
    sealed partial class App : Application
    {
        // TODO: Correct License key
        const string License = "XTUN3Q0ZHNTVGeWdNakFxZXRyM1F4NlFCV3NPQ3BJZjlBaFFBemJjd1AzT3d4Tk9WekN6MmRVTHltSW0zV2c9PQoKcHJ" + 
            "vZHVjdHM9c2RrLXdpbnBob25lLTMuKgpwcm9kdWN0SWQ9Yzg4MmQzOGEtNWMwOS00OTk0LTg3ZjAtODk4NzVjZGVlNTM5CndhdGVybWFyaz1udX" +
            "RpdGVxCnVzZXJLZXk9MTVjZDkxMzEwNzJkNmRmNjhiOGE1NGZlZGE1YjA0OTYK";

        MapView MapView { get; set; }

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        // Invoked when the application is launched normally by the end user.
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Register CARTO license
            bool registered = MapView.RegisterLicense(License);

            if (registered)
            {
                Carto.Utils.Log.ShowDebug = true;
            }

            MapView = new MapView();

            // Add base map

            // TODO: Crashes here for some reason
            //CartoOnlineVectorTileLayer baseLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDark);
            //MapView.Layers.Add(baseLayer);

            // Set default location and zoom
            Projection projection = MapView.Options.BaseProjection;

            MapPos berlin = projection.FromWgs84(new MapPos(13.38933, 52.51704));
            MapView.SetFocusPos(berlin, 0);
            MapView.SetZoom(10, 0);

            // Load vis
            string url = "http://documentation.carto.com/api/v2/viz/2b13c956-e7c1-11e2-806b-5404a6a683d5/viz.json";
            MapView.UpdateVis(url);

            Window.Current.Content = MapView;
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
}
