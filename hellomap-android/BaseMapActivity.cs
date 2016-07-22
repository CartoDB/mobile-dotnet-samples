using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.IO;

using Carto.Ui;
using Carto.Utils;
using Carto.Layers;
using HelloMap;
using Carto.Projections;

namespace CartoMobileSample
{
	[Activity (Label = "Hellomap", MainLauncher = true, Icon = "@drawable/icon")]
	public class BaseMapActivity : Activity
	{
		protected MapView mapView;
		internal Projection baseProjection;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Register license
			Carto.Utils.Log.ShowError = true;
			Carto.Utils.Log.ShowWarn = true;
			MapView.RegisterLicense("XTUN3Q0ZDL3RoWlRJdzNqTDVBWFlZR1BTTlh0OWdWRkFBaFFIaENuR2hhaVdyWHU2N1B4YmtYK1hXWnRHNEE9PQoKcHJvZHVjdHM9c2RrLXhhbWFyaW4tYW5kcm9pZC00LioKcGFja2FnZU5hbWU9Y29tLmNhcnRvLmhlbGxvbWFwLnhhbWFyaW4Kd2F0ZXJtYXJrPWRldmVsb3BtZW50CnZhbGlkVW50aWw9MjAxNi0wOC0yMQpvbmxpbmVMaWNlbnNlPTEK", ApplicationContext);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			mapView = (MapView)FindViewById (Resource.Id.mapView);

			baseProjection = new EPSG3857 ();

		}
	}
}
