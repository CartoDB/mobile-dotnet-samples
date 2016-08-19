using Android.App;
using Android.Widget;
using Android.OS;
using Carto.Ui;
using Carto.Layers;
using Carto.Core;
using System.Threading;
using Carto.Projections;
using Carto.Services;
using System;
using Shared;

namespace HelloMap.Droid
{
	[Activity(Label = "HelloMap", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		const string LICENSE = "XTUN3Q0ZEdkRBMy9IREh4K1RMK0J5UFZlM0gzY3pIdGVBaFFVTjgrQlQ0dDYrVXNWeGF2S2Z6V" +
			"URpNjJSQXc9PQoKcHJvZHVjdHM9c2RrLWFuZHJvaWQtNC4qCnBhY2thZ2VOYW1lPWNvbS5jYXJ0by5oZWxsb21hcC54YW" +
			"1hcmluLmFuZHJvaWQKd2F0ZXJtYXJrPWRldmVsb3BtZW50CnZhbGlkVW50aWw9MjAxNi0wOS0xNQpvbmxpbmVMaWNlbnNlPTEK";

		MapView MapView { get; set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Register license
			MapView.RegisterLicense(LICENSE, ApplicationContext);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			MapView = (MapView)FindViewById(Resource.Id.mapView);

			// Add base map
			CartoOnlineVectorTileLayer baseLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDark);
			MapView.Layers.Add(baseLayer);

			// Set default location and zoom
			Projection projection = MapView.Options.BaseProjection;

			MapPos berlin = projection.FromWgs84(new MapPos(13.38933, 52.51704));
			MapView.SetFocusPos(berlin, 0);
			MapView.SetZoom(10, 0);

			// Load vis
			string url = "http://documentation.carto.com/api/v2/viz/2b13c956-e7c1-11e2-806b-5404a6a683d5/viz.json";
			UpdateVis(url);
		}

		void UpdateVis(string url)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				MapView.Layers.Clear();

				// Create VIS loader
				CartoVisLoader loader = new CartoVisLoader();
				loader.DefaultVectorLayerMode = true;
				BasicCartoVisBuilder builder = new BasicCartoVisBuilder(MapView);

				try
				{
					loader.LoadVis(builder, url);
				}
				catch (Exception e)
				{
					Toast.MakeText(this, e.Message, ToastLength.Short);
				}

				MapPos tallinn = new MapPos(24.646469, 59.426939);
				MapView.AddMarkerToPosition(tallinn);
			});
		}
	}
}


