
using System;
using Android.App;
using Android.Support.V7.App;
using Shared.Droid;
using Shared;

namespace AdvancedMap.Droid
{
    [Activity]
    public class ReverseGeocodingActivity : PackageDownloadBaseActivity
    {
        public Geocoding GeocodingClient { get { return Client as Geocoding; } }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ContentView = new BaseGeocodingView(this);
            SetContentView(ContentView);

            string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            Client = new Geocoding(path);
        }        

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }
    }
}
