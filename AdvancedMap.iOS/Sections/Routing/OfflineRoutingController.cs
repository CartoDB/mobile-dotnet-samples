using System;
using System.Collections.Generic;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Routing;
using CoreGraphics;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class OfflineRoutingController : BaseRoutingController
	{
		public override string Name { get { return "Offline Routing"; } }

		public override string Description { get { return "Offline routing with OpenStreetMap data packages"; } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            string text = "Long click on the map to set route start point";
            ContentView.Banner.Show(text);

			SetOnlineMode();
			ContentView.SetOnlineMode();
		}

        public override void SetOnlineMode()
		{
			Routing.Service = new ValhallaOnlineRoutingService(Sources.MapzenApiKey);
		}

		public override void SetOfflineMode()
		{
			string text = "Click the globa icon to download routing packages";
			ContentView.Banner.Show(text);
			Routing.Service = new PackageManagerValhallaRoutingService(Routing.Manager);
		}
	}
}
