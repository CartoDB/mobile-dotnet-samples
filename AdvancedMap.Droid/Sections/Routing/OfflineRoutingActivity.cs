using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.Views;
using Carto.DataSources;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Routing;
using Carto.Styles;
using Carto.VectorElements;

using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Offline routing", Description = "Offline routing with OpenStreetMap data packages")]
	public class OfflineRoutingActivity : BaseRoutingActivity
	{   
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            SetOnlineMode();
		}

        protected override void OnResume()
        {
            base.OnResume();

            string text = "Long click on the map to set your route start point";
            ContentView.Banner.Show(text);
        }

        protected override void SetOnlineMode()
        {
            Routing.Service = new CartoOnlineRoutingService(Sources.NutiteqRouting);
        }

        protected override void SetOfflineMode()
        {
            // Create offline routing service connected to package manager
            Routing.Service = new PackageManagerValhallaRoutingService(Routing.Manager);
        }
	}
}
