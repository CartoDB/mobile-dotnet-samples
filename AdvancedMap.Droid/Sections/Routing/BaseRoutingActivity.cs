using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Graphics;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Routing;
using Carto.Styles;
using Carto.Ui;
using Carto.VectorElements;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	public class BaseRoutingActivity : MapBaseActivity
	{
		protected RouteMapEventListener MapListener { get; set; }

		protected Routing Routing;

		protected void Initialize(MapView map)
		{
			// Set route listener
			MapListener = new RouteMapEventListener();
			map.MapEventListener = MapListener;

			// Virtual method overridden in child classes in order to keep layer order correct
			SetBaseLayer();

			Routing = new Routing(map, BaseProjection);

			Alert("Long-press on map to set route start and finish");

			Bitmap olmarker = CreateBitmap(Resource.Drawable.olmarker);
			Bitmap directionUp = CreateBitmap(Resource.Drawable.direction_up);
			Bitmap directionUpLeft = CreateBitmap(Resource.Drawable.direction_upthenleft);
			Bitmap directionUpRight = CreateBitmap(Resource.Drawable.direction_upthenright);

			Color green = new Color(Android.Graphics.Color.Green);
			Color red = new Color(Android.Graphics.Color.Red);
			Color white = new Color(Android.Graphics.Color.White);

			Routing.SetSourcesAndElements(olmarker, directionUp, directionUpLeft, directionUpRight, green, red, white);
		}

		protected virtual void SetBaseLayer()
		{
			throw new NotImplementedException();	
		}

		protected override void OnResume()
		{
			base.OnResume();

			MapListener.StartPositionClicked += OnStartPositionClick;
			MapListener.StopPositionClicked += OnStopPositionClick;
		}

		protected override void OnPause()
		{
			base.OnPause();

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
                    RoutingComplete();
				});
			});
		}

        public virtual void RoutingComplete() { }
	}
}
