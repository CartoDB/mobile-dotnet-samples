
using System;
using Carto.Layers;
using Carto.Ui;
using Xamarin.Forms;
using Carto.VectorElements;
using Carto.Core;
using Carto.Projections;
using Carto.DataSources;

// minimal platform-specific code to reference MapView
#if __ANDROID__
using Xamarin.Forms.Platform.Android;
using HelloMap.Forms.Droid;
#elif WINDOWS_PHONE
using Xamarin.Forms.Platform.UWP;
using HelloMap.Forms.WP;
#elif __IOS__
using Xamarin.Forms.Platform.iOS;
#endif

namespace HelloMap.Forms
{
	public class MainPage : ContentPage
	{
		MapView MapView { get; set; }

		public MainPage()
		{
            // Be sure to register your license in native code or in an #if case.
            // Even if your package names are identical, licenses are platform-specific.
            // This sample register's license in App.xaml.cs

			AbsoluteLayout view = new AbsoluteLayout();

			// Since MapView is a native element, initialize and set its size natively
			// minimal platform-specific code is needed to create MapView

#if __ANDROID__
			MapView = new MapView(Xamarin.Forms.Forms.Context);
#elif WINDOWS_PHONE
            MapView = new MapView();

            Windows.Foundation.Rect bounds = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds;
            MapView.Width = bounds.Width;
            MapView.Height = bounds.Height;
#elif __IOS__
            MapView = new MapView();
			// Set ScreenBounds in AppDelegate so they would be conveniently available here
			MapView.Frame = iOS.AppDelegate.ScreenBounds;
#endif


			// all the remaining usage of MapView is cross-platform
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
			MapView.AddMarkerToPosition(berlin);

			// Good place to set the sizes and positions of Children, 
			// similar to LayoutSubviews of iOS and OnLayout of Android
			view.SizeChanged += OnSizeChanged;

			Button button = new Button();
			button.BackgroundColor = Color.FromRgb(117, 58, 97);
			button.TextColor = Color.White;
			button.Text = "Hide Marker";

			button.VerticalOptions = LayoutOptions.EndAndExpand;
			button.HorizontalOptions = LayoutOptions.EndAndExpand;

			view.Children.Add(button);

			button.Clicked += OnButtonClick;
		}

		void OnSizeChanged(object sender, EventArgs e)
		{
			AbsoluteLayout view = sender as AbsoluteLayout;
			// In our case, we know the first element of the view is the map and second is the button
			Button button = (Button)view.Children[1];

			int margin = 5;

			button.WidthRequest = 150;
			button.HeightRequest = 60;
			button.Margin = new Thickness(5, 5, 5, 5);

			// Set button to bottom right corner, with some margin as well
			button.Margin = new Thickness(
				view.Width - (button.WidthRequest + margin), 
				view.Height - (button.HeightRequest + margin), 
				margin, 
				margin
			);
		}

		void OnButtonClick(object sender, EventArgs e)
		{
			Button button = (Button)sender;

			// We know that the second layer is a Vectorlayer and its DataSource is a LocalVectorDataSource
			// cf. Methods in Extensions.cs
			var source = ((MapView.Layers[1] as VectorLayer).DataSource as LocalVectorDataSource);
			// This method gets all the elements, and this sample only contains one Marker
			var marker = (Marker)source.GetAll()[0];

			if (button.Text.Equals("Hide Marker"))
			{
				button.Text = "Show Marker";
				marker.Visible = false;
			}
			else
			{
				button.Text = "Hide Marker";
				marker.Visible = true;
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			// We know that the second layer is a Vectorlayer and its DataSource is a LocalVectorDataSource
			// cf. Methods in Extensions.cs
			var source = ((MapView.Layers[1] as VectorLayer).DataSource as LocalVectorDataSource);
			// This method gets all the elements, and this sample only contains one Marker
			var marker = (Marker)source.GetAll()[0];

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
