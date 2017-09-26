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

            Alert("This sample uses an online map, but downloads routing packages");

            Alert("Click on the menu to see a list of countries that can be downloaded");

			SetOnlineMode();
			ContentView.SetOnlineMode();
		}

        public override void SetOnlineMode()
		{
			Routing.Service = new ValhallaOnlineRoutingService(Sources.MapzenApiKey);
		}

		public override void SetOfflineMode()
		{
			// Create offline routing service connected to package manager
			Routing.Service = new PackageManagerValhallaRoutingService(Routing.Manager);
		}
	}
}
