using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using HockeyApp.Android;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity(MainLauncher = true)]
	public class LauncherListActivity : ListActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Title = "CARTO Samples";

			SetContentView(Resource.Layout.List);

			if (ActionBar != null)
			{
				ActionBar.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable { Color = Colors.ActionBar });
			}

			ListView.Adapter = new MapListAdapter(this, Samples.List);
			ListView.SetBackgroundColor(Color.Black);

			UpdateManager.Register(this, MapApplication.HockeyId);
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
			Type sample = Samples.FromPosition(position);

			if (sample.IsHeader()) {
				// Group headers aren't clickable
				return;
			}

			StartActivity(new Intent(this, sample));
		}
	}
}

