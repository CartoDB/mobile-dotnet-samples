using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using HockeyApp.Android;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity(MainLauncher = true)]
	public class LauncherListActivity : Activity
	{
		MapGalleryView ContentView;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Title = "Carto Samples";

			ActionBar.SetBackgroundDrawable(new ColorDrawable { Color = Colors.CartoNavy });

			ContentView = new MapGalleryView(this);
			SetContentView(ContentView);

			ContentView.AddRows(Samples.Items);

			//UpdateManager.Register(this, MapApplication.HockeyId);
		}

		protected override void OnResume()
		{
			base.OnResume();

			ContentView.RowClick += OnRowClicked;
		}

		protected override void OnPause()
		{
			base.OnPause();

			ContentView.RowClick -= OnRowClicked;
		}


		void OnRowClicked(object sender, EventArgs e)
		{
			GalleryRow row = (GalleryRow)sender;

			StartActivity(new Intent(this, row.Activity));
		}
	}
}

