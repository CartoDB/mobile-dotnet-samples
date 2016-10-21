using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity(Label = "CARTO Mobile Samples", MainLauncher = true)]
	public class LauncherListActivity : ListActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.List);

			ListView.Adapter = new MapListAdapter(this, Samples.List);
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

