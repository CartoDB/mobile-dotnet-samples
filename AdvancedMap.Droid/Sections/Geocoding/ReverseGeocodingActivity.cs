
using System;
using Android.App;
using Android.Support.V7.App;
using Shared.Droid;
using Shared;
using Carto.Geocoding;

namespace AdvancedMap.Droid
{
    [Activity]
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
        }        

        protected override void OnResume()
        {
            base.OnResume();

            ContentView.MapView.MapEventListener = Listener;
            Listener.ResultFound += OnGeocodingResultFound;
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
				Alert("Couldn't find any addresses. Are you sure you have downloaded the region you're trying to reverse geocode?");
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
            Listener.Service = new PeliasOnlineReverseGeocodingService(ApiKey);
        }

        protected override void SetOfflineMode()
        {
            Listener.Service = new PackageManagerReverseGeocodingService(GeocodingClient.Manager);
        }
    }
}
