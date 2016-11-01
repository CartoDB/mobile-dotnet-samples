using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HockeyApp.Android;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity(MainLauncher = true)]
	public class LauncherListActivity : ListActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Title = "Advanced Samples";

			ActionBar.SetBackgroundDrawable(new ColorDrawable { Color = Colors.ActionBar });

			SetContentView(Resource.Layout.List);

			MapRowView.RowId = Resource.Id.MapListCell;
			ListView.Id = Resource.Id.MapListView;

			ListView.Adapter = new MapListAdapter(this, Samples.List);

			UpdateManager.Register(this, MapApplication.HockeyAppId);
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

