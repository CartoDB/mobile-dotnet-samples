
using System;
using Android.App;
using Android.Support.V7.App;
using Shared.Droid;
using Shared;
using Carto.Geocoding;

namespace AdvancedMap.Droid
{
    [Activity]
    [ActivityData(Title = "Reverse Geocoding", Description = "Click an area on the map to find out more about it")]
    public class ReverseGeocodingActivity : BaseGeocodingActivity
    {
        public ReverseGeocodingEventListener Listener { get; set; }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ContentView = new ReverseGeocodingView(this);
            SetContentView(ContentView);

            GeocodingClient.Projection = ContentView.Projection;

            Listener = new ReverseGeocodingEventListener(ContentView.Projection);

            SetOnlineMode();
            ContentView.SetOnlineMode();
        }        

        protected override void OnResume()
        {
            base.OnResume();

            ContentView.MapView.MapEventListener = Listener;
            Listener.ResultFound += OnGeocodingResultFound;

			string text = "Click on the map to find out more about a location";
			ContentView.Banner.Show(text);
        }

        protected override void OnPause()
        {
            base.OnPause();

			ContentView.MapView.MapEventListener = null;
			Listener.ResultFound -= OnGeocodingResultFound;
        }

		void OnGeocodingResultFound(object sender, EventArgs e)
		{
			GeocodingResult result = (GeocodingResult)sender;

			if (result == null)
			{
                string text = "Couldn't find any addresses. Please try again";
                ContentView.Banner.Show(text);
                return;
			}

			string title = "";
			string description = result.ToString();
			bool goToPosition = false;

            var view = ContentView as BaseGeocodingView;
            view.GeocodingSource.ShowResult(ContentView.MapView, result, title, description, goToPosition);
		}

        protected override void SetOnlineMode()
        {
			Listener.Service = new PeliasOnlineReverseGeocodingService(Sources.MapzenApiKey);
        }

        protected override void SetOfflineMode()
        {
            Listener.Service = new PackageManagerReverseGeocodingService(GeocodingClient.Manager);
        }
    }
}
