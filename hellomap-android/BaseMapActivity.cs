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
	[Activity (Label = "Hello Map", MainLauncher = true, Icon = "@drawable/icon")]
	public class BaseMapActivity : Activity
	{
		const string LICENSE = "XTUN3Q0ZDL3RoWlRJdzNqTDVBWFlZR1BTTlh0OWdWRkFBaFFIaENuR2hhaVdyWHU2N1B4YmtYK1hXWnRHNEE9" +
			"PQoKcHJvZHVjdHM9c2RrLXhhbWFyaW4tYW5kcm9pZC00LioKcGFja2FnZU5hbWU9Y29tLmNhcnRvLmhlbGxvbWFwLnhhbWFyaW4Kd2F0" +
			"ZXJtYXJrPWRldmVsb3BtZW50CnZhbGlkVW50aWw9MjAxNi0wOC0yMQpvbmxpbmVMaWNlbnNlPTEK";

		protected MapView MapView { get; set; }
		internal Projection BaseProjection { get; set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Log.ShowError = true;
			Log.ShowWarn = true;

			// Register license
			MapView.RegisterLicense(LICENSE, ApplicationContext);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			MapView = (MapView)FindViewById(Resource.Id.mapView);

			BaseProjection = new EPSG3857();

			// Hide Back in MainActivity
			if (this.GetType() == typeof(LauncherListActivity))
			{
				ActionBar.SetDisplayHomeAsUpEnabled(false);
			}
			else {
				ActionBar.SetDisplayHomeAsUpEnabled(true);
			}
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home)
			{
				OnBackPressed();
				return true;
			}

			return base.OnOptionsItemSelected(item);
		}
	}
}
