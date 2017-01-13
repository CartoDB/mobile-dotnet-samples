
using System;
using Carto.Layers;
using Carto.Ui;
using Xamarin.Forms;
using Carto.VectorElements;
using Carto.Core;
using Carto.Projections;

#if __ANDROID__
using Xamarin.Forms.Platform.Android;
using HelloMap.Forms.Droid;
#elif WINDOWS_PHONE
using Xamarin.Forms.Platform.UWP;
using HelloMap.Forms.WP;
#else
using Xamarin.Forms.Platform.iOS;
#endif

namespace HelloMap.Forms
{
	public class MainPage : ContentPage
	{
		MapView MapView { get; set; }

		Marker marker;

		public MainPage()
		{
            // Be sure to register your license in native code or in an #if case.
            // Even if your package names are identical, licenses are platform-specific.
            // This sample register's license in App.xaml.cs

			AbsoluteLayout view = new AbsoluteLayout();

#if __ANDROID__
			MapView = new MapView(Xamarin.Forms.Forms.Context);
#elif WINDOWS_PHONE
            MapView = new MapView();

            Windows.Foundation.Rect bounds = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds;
            MapView.Width = bounds.Width;
            MapView.Height = bounds.Height;
#else
            MapView = new MapView();
			// Set ScreenBounds in AppDelegate so they would be conveniently available here
			MapView.Frame = iOS.AppDelegate.ScreenBounds;
#endif
			view.Children.Add(MapView.ToView());
			Content = view;

			// Add default base layer
			var baseLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);
			MapView.Layers.Add(baseLayer);

			// Set projection
			Projection projection = MapView.Options.BaseProjection;

			// Set default position and zoom
			// Change projection of map so coordinates would fit on a mercator map
			MapPos berlin = MapView.Options.BaseProjection.FromWgs84(new MapPos(13.38933, 52.51704));
			MapView.SetFocusPos(berlin, 0);
			MapView.SetZoom(10, 0);

			// Custom extension method. cf Extensions.cs
			marker = MapView.AddMarkerToPosition(berlin);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			// Add simple event listener that changes size and/or color on map click
			MapView.MapEventListener = new MapListener(marker);
		}

		protected override void OnDisappearing()
		{
			// Remove listener when view disappears to avoid memory leaks
			MapView.MapEventListener = null;

			base.OnDisappearing();
		}
	}
}
