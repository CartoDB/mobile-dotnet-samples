
using System;
using Android.App;
using Android.Support.V7.App;
using Shared.Droid;

namespace AdvancedMap.Droid
{
    [Activity]
    public class ReverseGeocodingActivity : Activity
    {
        MapBaseView ContentView { get; set; }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ContentView = new BaseGeocodingView(this);
            SetContentView(ContentView);
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
