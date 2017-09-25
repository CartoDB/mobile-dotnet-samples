
using System;
using Carto.Core;
using Carto.Graphics;
using Carto.Routing;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class BaseRoutingController : MapBaseController
	{
		protected RouteMapEventListener MapListener { get; set; }

		protected Routing Routing;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Virtual method overridden in child classes in order to keep layer order correct
			SetBaseLayer();

			// Set route listener
			MapListener = new RouteMapEventListener();
			
			Routing = new Routing(MapView, null);

			Alert("Long-press on map to set route start and finish");

			Bitmap olmarker = CreateBitmap("icons/olmarker.png");
			Bitmap directionUp = CreateBitmap("icons/direction_up.png");
			Bitmap directionUpLeft = CreateBitmap("icons/direction_upthenleft.png");
			Bitmap directionUpRight = CreateBitmap("icons/direction_upthenright.png");

			Color green = new Color(0, 255, 0, 255);
			Color red = new Color(255, 0, 0, 255);
			Color white = new Color(255, 255, 255, 255);

			Routing.SetSourcesAndElements(olmarker, directionUp, directionUpLeft, directionUpRight, green, red, white);
		}

		protected virtual void SetBaseLayer()
		{
			throw new NotImplementedException();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			MapListener.StartPositionClicked += OnStartPositionClick;
			MapListener.StopPositionClicked += OnStopPositionClick;

			MapView.MapEventListener = MapListener;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			MapListener.StartPositionClicked -= OnStartPositionClick;
			MapListener.StopPositionClicked -= OnStopPositionClick;

			MapView.MapEventListener = null;
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
				long time = DateTime.Now.Millisecond;

				RoutingResult result = null;

				try
				{
					result = Routing.GetResult(startPos, stopPos);
				}
				catch(Exception e)
				{
					Console.WriteLine(e.Message);
				}

                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();

				InvokeOnMainThread(() =>
				{
					if (result == null)
					{
						Alert("Routing failed. Have you downloaded offline package for the region?");
						return;
					}

                    Alert(Routing.GetMessage(result, watch.ElapsedMilliseconds));
                    watch.Stop();

					Color lineColor = new Color(0, 122, 255, 255);
					Routing.Show(result);
                    RoutingComplete();
				});
			});
		}

        public virtual void RoutingComplete()
        {
            // Implementation in RouteSearchController 
            // where attractions are added after the route is calculated
        }
	}
}
