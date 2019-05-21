using Android.App;
using Android.Widget;
using Android.OS;
using Carto.Ui;
using Carto.Layers;
using Carto.Core;
using Carto.Projections;
using Shared;
using Carto.VectorElements;

namespace HelloMap.Droid
{
	[Activity(Label = "HelloMap", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		const string LICENSE = "XTUN3Q0ZIaGlMNGZFOU5OcmNYUVVoSHhYMDM1dXdmd1hBaFFKR2FDa1puc2RTdk5PWDVqT1FyL2JhU0c4MVE9PQoKYXBwVG9rZW49ZGU3N2ZlYzgtN2MyNC00NWI0LWEwZDItODM0Yjc4ODAwNjAyCnBhY2thZ2VOYW1lPWNvbS5jYXJ0by5oZWxsb21hcApvbmxpbmVMaWNlbnNlPTEKcHJvZHVjdHM9c2RrLXhhbWFyaW4tYW5kcm9pZC00LioKd2F0ZXJtYXJrPWN1c3RvbQo=";

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
            CartoOnlineVectorTileLayer baseLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
			MapView.Layers.Add(baseLayer);

			// Set projection
			Projection projection = MapView.Options.BaseProjection;

			// Set default position and zoom
			// Change projection of map so coordinates would fit on a mercator map
			MapPos berlin = MapView.Options.BaseProjection.FromWgs84(new MapPos(13.38933, 52.51704));
			MapView.SetFocusPos(berlin, 0);
			MapView.SetZoom(10, 0);

			Marker marker = MapView.AddMarkerToPosition(berlin);

			// Add simple event listener that changes size and/or color on map click
			MapView.MapEventListener = new HelloMapEventListener(marker);
		}

        protected override void OnResume()
        {
            base.OnResume();

            Toast.MakeText(this, "Click on the map to update your marker", ToastLength.Long).Show();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Disconnect listener to avoid leaks
            MapView.MapEventListener = null;
        }
    }
}


