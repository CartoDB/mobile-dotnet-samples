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

using Carto.Projections;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity (Icon = "@mipmap/icon")]
	public class BaseMapActivity : Activity
	{
		const string LICENSE = "XTUN3Q0ZFMmhzUWEwdlZwNlJNVW5kL1hMRExMYXNYVUxBaFFhKzlSK2drYjEzNnBhSTJzY1c5aDBkL2licFE9PQo" +
			"KcHJvZHVjdHM9c2RrLXhhbWFyaW4tYW5kcm9pZC00LioKcGFja2FnZU5hbWU9Y29tLmNhcnRvLmFkdmFuY2VkbWFwLnhhbWFyaW4uZHJvaW" +
			"QKd2F0ZXJtYXJrPWRldmVsb3BtZW50CnZhbGlkVW50aWw9MjAxNi0wOS0xOApvbmxpbmVMaWNlbnNlPTEK";

		protected MapView MapView { get; set; }
		internal Projection BaseProjection { get; set; }
		protected TileLayer BaseLayer { get; set; }

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

			Title = GetType().GetTitle();

			ActionBar.SetDisplayHomeAsUpEnabled(true);
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
