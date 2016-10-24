using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	/// <summary>
	/// Shows list of demo Activities. Enables to open pre-launch activity for file picking.
	/// This is the "main" of samples
	/// 
	/// @author jaak
	/// @translated by m@t
	/// </summary>
	[Activity(Label = "Advanced Mobile Samples", MainLauncher = true)]
	public class LauncherListActivity : ListActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.List);

			MapRowView.RowId = Resource.Id.MapListCell;
			ListView.Id = Resource.Id.MapListView;

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

