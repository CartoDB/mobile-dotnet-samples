using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Geometry;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Routing;
using Carto.Search;
using Carto.Styles;
using Carto.VectorElements;
using Shared;
using Shared.iOS;

namespace AdvancedMap.iOS
{
    public class RouteSearchController : PackageDownloadBaseController
    {
		public override string Name { get { return "Route Search"; } }

		public override string Description { get { return "Find points of interest near a route"; } }

		RouteSearch Search { get; set; }

		Routing Routing { get; set; }

		protected RouteMapEventListener MapListener { get; set; }

		public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			ContentView = new RouteSearchView();
			View = ContentView;

			Client = new BasePackageManagerClient();

			Routing = new Routing(ContentView.MapView, ContentView.Projection);
			Routing.ShowTurns = false;
			Routing.SetSourcesAndElements();

			Client.Manager = Routing.PackageManager;

			Search = new RouteSearch(ContentView.MapView, (ContentView as RouteSearchView).BaseLayer);

			MapListener = new RouteMapEventListener();

            SetOnlineMode();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

			Search.AddListeners();

			ContentView.MapView.MapEventListener = MapListener;
			MapListener.StartPositionClicked += OnStartPositionClick;
			MapListener.StopPositionClicked += OnStopPositionClick;
			MapListener.SingleTapped += OnSingleTap;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

			Search.RemoveListeners();

			ContentView.MapView.MapEventListener = null;
			MapListener.StartPositionClicked -= OnStartPositionClick;
			MapListener.StopPositionClicked -= OnStopPositionClick;
			MapListener.SingleTapped -= OnSingleTap;
        }

        void OnSingleTap(object sender, EventArgs e)
        {
            Search.ClearPopups();
        }

		public override void SetOnlineMode()
		{
			var source = Sources.OnlineRouting + Sources.TransportMode_Car;
			Routing.Service = new CartoOnlineRoutingService(source);
		}

		public override void SetOfflineMode()
		{
			Routing.Service = new PackageManagerValhallaRoutingService(Client.Manager);
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
				var watch = new System.Diagnostics.Stopwatch();
				watch.Start();

				RoutingResult result = Routing.GetResult(startPos, stopPos);

				// Update response in UI thread
                InvokeOnMainThread(() =>
				{
					if (result == null)
					{
						Alert("Routing failed");
						return;
					}

					Alert(Routing.GetMessage(result, watch.ElapsedMilliseconds));
					watch.Stop();

					Routing.Show(result);

					FeatureCollection collection = Routing.routeDataSource.GetFeatureCollection();
					Search.FindAttractions(collection);
				});
			});
		}
    }
}
