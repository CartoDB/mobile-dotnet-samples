
using System;
using Carto.Core;
using Carto.Graphics;
using Carto.Routing;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
    public class BaseRoutingController : PackageDownloadBaseController
	{
		protected RouteMapEventListener MapListener { get; set; }

		protected Routing Routing
		{
			get { return Client as Routing; }
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            ContentView = new PackageDownloadBaseView();
            View = ContentView;

			string folder = GetPackageFolder(Routing.PackageFolder);
            Client = new Routing(ContentView.MapView, folder);

            Routing.SetSourcesAndElements();

			// Set route listener
			MapListener = new RouteMapEventListener();
			
			Alert("Long-press on map to set route start and finish");

            ContentView.SetOnlineMode();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			MapListener.StartPositionClicked += OnStartPositionClick;
			MapListener.StopPositionClicked += OnStopPositionClick;

			ContentView.MapView.MapEventListener = MapListener;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			MapListener.StartPositionClicked -= OnStartPositionClick;
			MapListener.StopPositionClicked -= OnStopPositionClick;

            ContentView.MapView.MapEventListener = null;
		}

		protected void OnStartPositionClick(object sender, RouteMapEventArgs e)
		{
			Routing.SetStartMarker(e.ClickPosition);
		}

		protected void OnStopPositionClick(object sender, RouteMapEventArgs e)
		{
			Routing.SetStopMarker(e.ClickPosition);
			ShowRoute(e.StartPosition, e.StopPosition);
		}

		public void ShowRoute(MapPos startPos, MapPos stopPos)
		{
			// Run routing in background
			System.Threading.Tasks.Task.Run(() =>
			{
				RoutingResult result = Routing.GetResult(startPos, stopPos);

				InvokeOnMainThread(() =>
				{
					if (result == null)
					{
						Alert("Routing failed. Have you downloaded offline package for the region?");
						return;
					}

                    Routing.Show(result);

                    Alert(Routing.GetMessage(result));
				});
			});
		}
	}
}
