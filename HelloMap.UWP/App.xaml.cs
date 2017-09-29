
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
        const string License = "XTUN3Q0ZFQ2NRZDF0ZmoyYXhDd1JrSk5rdEkyK2FSS1VBaFI4Q3k0c0Vla2NTSWxEcEZLWGJCQVpIcUs3WGc9PQoKYXBwVG9rZW49NTU4MmEzMDktNDJmNS00YmM4LWJlNTEtOTlhMmEwYmJjYzc3CnByb2R1Y3RJZD1jODgyZDM4YS01YzA5LTQ5OTQtODdmMC04OTg3NWNkZWU1MzkKcHJvZHVjdHM9c2RrLXdpbnBob25lLTQuKgpvbmxpbmVMaWNlbnNlPTEKd2F0ZXJtYXJrPWNhcnRvZGIK";

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
            CartoOnlineVectorTileLayer baseLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDarkmatter);
            MapView.Layers.Add(baseLayer);

            // Set default location and zoom
            Projection projection = MapView.Options.BaseProjection;

            MapPos tallinn = projection.FromWgs84(new MapPos(24.646469, 59.426939));
            MapView.AddMarkerToPosition(tallinn);

            MapView.SetFocusPos(tallinn, 0);
            MapView.SetZoom(3, 0);

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
