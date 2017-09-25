
using System;
using Android.App;
using Carto.Core;
using Carto.Geometry;
using Carto.Routing;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
    [Activity]
    [ActivityData(Title = "Route Search", Description = "Finds points of interest near your route")]
    public class RouteSearchActivity : PackageDownloadBaseActivity
    {
        RouteSearch Search { get; set; }

        Routing Routing { get { return Client as Routing; } }
		
        protected RouteMapEventListener MapListener { get; set; }

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ContentView = new RouteSearchView(this);
            SetContentView(ContentView);

            Client = new Routing(ContentView.MapView, null);
			Routing.ShowTurns = false;
            Routing.SetSourcesAndElements(this);

            Search = new RouteSearch(ContentView.MapView, (ContentView as RouteSearchView).BaseLayer);

            MapListener = new RouteMapEventListener();

            SetOnlineMode();
            ContentView.HidePackageDownloadButtons();
        }

        protected override void OnResume()
        {
            base.OnResume();

			Search.AddListeners();

            ContentView.MapView.MapEventListener = MapListener;
            MapListener.StartPositionClicked += OnStartPositionClick;
            MapListener.StopPositionClicked += OnStopPositionClick;
            MapListener.SingleTapped += OnSingleTap;
        }

        protected override void OnPause()
        {
            base.OnPause();

			Search.RemoveListeners();

            ContentView.MapView.MapEventListener = null;
			MapListener.StartPositionClicked -= OnStartPositionClick;
			MapListener.StopPositionClicked -= OnStopPositionClick;
			MapListener.SingleTapped -= OnSingleTap;
        }

        protected override void SetOnlineMode()
        {
            Routing.Service = new ValhallaOnlineRoutingService(Sources.MapzenApiKey);
        }

		void OnSingleTap(object sender, EventArgs e)
		{
			Search.ClearPopups();
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
				RunOnUiThread(() =>
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
