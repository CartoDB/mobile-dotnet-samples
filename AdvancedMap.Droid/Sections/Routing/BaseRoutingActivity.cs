using System;
using Android.OS;
using Carto.Core;
using Carto.Graphics;
using Carto.Layers;
using Carto.Routing;
using Carto.Ui;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
    public class BaseRoutingActivity : PackageDownloadBaseActivity
	{
		protected RouteMapEventListener MapListener { get; set; }

		protected Routing Routing
        {
            get { return Client as Routing; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ContentView = new OfflineRoutingView(this);
            SetContentView(ContentView);

            string folder = GetPackageFolder(Routing.PackageFolder);
            Client = new Routing(ContentView.MapView, folder);

            MapListener = new RouteMapEventListener();

            Alert("Long-press on map to set route start and finish");

            Bitmap olmarker = CreateBitmap(Resource.Drawable.olmarker);
            Bitmap directionUp = CreateBitmap(Resource.Drawable.direction_up);
            Bitmap directionUpLeft = CreateBitmap(Resource.Drawable.direction_upthenleft);
            Bitmap directionUpRight = CreateBitmap(Resource.Drawable.direction_upthenright);

            Color green = new Color(Android.Graphics.Color.Green);
            Color red = new Color(Android.Graphics.Color.Red);
            Color white = new Color(Android.Graphics.Color.White);

            ContentView.SetOnlineMode(delegate
            {
                Routing.SetSourcesAndElements(olmarker, directionUp, directionUpLeft, directionUpRight, green, red, white);
            });
        }

		protected override void OnResume()
		{
			base.OnResume();

			ContentView.MapView.MapEventListener = MapListener;

			MapListener.StartPositionClicked += OnStartPositionClick;
			MapListener.StopPositionClicked += OnStopPositionClick;
		}

		protected override void OnPause()
		{
			base.OnPause();

			ContentView.MapView.MapEventListener = null;

			MapListener.StartPositionClicked -= OnStartPositionClick;
			MapListener.StopPositionClicked -= OnStopPositionClick;
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

				// Update response in UI thread
				RunOnUiThread(() =>
				{
					if (result == null)
					{
						Alert("Routing failed");
						return;
					}

					Alert(Routing.GetMessage(result));
					
					Routing.Show(result);
                    RoutingComplete();
				});
			});
		}

        public virtual void RoutingComplete() { }
	}
}
