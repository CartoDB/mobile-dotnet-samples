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
using HelloMap;


namespace CartoMobileSample
{
	/// <summary>
	/// Shows list of demo Activities. Enables to open pre-launch activity for file picking.
	/// This is the "main" of samples
	/// 
	/// @author jaak
	/// @translated by m@t
	/// </summary>
	[Activity(Label = "CARTO Mobile Samples", MainLauncher = true)]
	public class LauncherListActivity : ListActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.List);

			int id = Android.Resource.Layout.SimpleListItem1;

			ListView.Adapter = new ArrayAdapter<string>(this, id, Samples.AsStringArray);
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
			Intent intent = new Intent(this, Samples.FromPosition(position));
			this.StartActivity(intent);
		}
	}
}

