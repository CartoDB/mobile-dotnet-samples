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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using Carto.Core;
using Carto.Graphics;
using Carto.DataSources;
using Carto.Projections;
using Carto.Layers;
using Carto.Styles;
using Carto.VectorElements;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace HelloMap
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class HelloMapApp : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public HelloMapApp()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (mPage == null)
            {
                Carto.Utils.Log.ShowDebug = true;
                Carto.Utils.Log.ShowInfo = true;
                Carto.Utils.Log.ShowError = true;

                Carto.Utils.Log.Error("aaa");

                // Register Nutiteq app license
                var licenseOk = Carto.Ui.MapView.RegisterLicense("XTUN3Q0ZHNTVGeWdNakFxZXRyM1F4NlFCV3NPQ3BJZjlBaFFBemJjd1AzT3d4Tk9WekN6MmRVTHltSW0zV2c9PQoKcHJvZHVjdHM9c2RrLXdpbnBob25lLTMuKgpwcm9kdWN0SWQ9Yzg4MmQzOGEtNWMwOS00OTk0LTg3ZjAtODk4NzVjZGVlNTM5CndhdGVybWFyaz1udXRpdGVxCnVzZXJLZXk9MTVjZDkxMzEwNzJkNmRmNjhiOGE1NGZlZGE1YjA0OTYK");
                if (licenseOk)
                {
                    Carto.Utils.Log.ShowDebug = true;
                }

                // Get asset folder for mbtiles file
                var importPackageName = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "Assets\\world_ntvt_0_4.mbtiles");

                // Create folder for packages
                await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("packages", Windows.Storage.CreationCollisionOption.OpenIfExists);
                var packageFolder = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "packages");

                // Create map view and initialize (actual initialization code is shared between Android/iOS/WinPhone platforms)
                mPage = new Carto.Ui.MapView();
                //NutiteqSample.MapSetup.InitializePackageManager(packageFolder, importPackageName, mPage, "");
                NutiteqSample.MapSetup.AddMapOverlays(mPage);
            }

            // Place the page in the current window and ensure that it is active.
            Windows.UI.Xaml.Window.Current.Content = mPage;
            Windows.UI.Xaml.Window.Current.Activate();
        }

        private Carto.Ui.MapView mPage;
    }
}
