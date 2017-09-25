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

			Alert("This sample uses an online map, but downloads routing packages");

			Alert("Click on the menu to see a list of countries that can be downloaded");

            SetOnlineMode();
		}

        protected override void SetOnlineMode()
        {
            Routing.Service = new ValhallaOnlineRoutingService(Sources.MapzenApiKey);
        }

        protected override void SetOfflineMode()
        {
            // Create offline routing service connected to package manager
            Routing.Service = new PackageManagerValhallaRoutingService(Routing.Manager);
        }
	}
}
