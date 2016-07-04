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


namespace NutiteqSample
{
	/// <summary>
	/// Shows list of demo Activities. Enables to open pre-launch activity for file picking.
	/// This is the "main" of samples
	/// 
	/// @author jaak
	/// @translated by m@t
	/// </summary>
	[Activity (Label = "Nutiteq Samples", MainLauncher = true)]			
	public class LauncherListActivity : ListActivity
	{
		// list of demos

		private static List<Type> _samples = new List<Type>(new Type[] {
					typeof ( OfflineMap ),
					typeof ( OnlineMap ),
					typeof ( MapOverlays ),
					typeof ( ClusteredGeoJSONCaptitals ),
					typeof ( GpsLocationMap ),
					typeof ( OfflineRouting )
		});

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate ( bundle );
 
			SetContentView ( Resource.Layout.List );
			ListView listView = this.ListView;
			listView.Adapter = new ArrayAdapter<String> ( this,  Android.Resource.Layout.SimpleListItem1, GetStringArray () );
		}

		private String[] GetStringArray () 
		{
			String[] sampleNames = new String[_samples.Count];
			for( int i = 0; i < _samples.Count; i++ ) 
			{
				sampleNames[i] = _samples[i].Name;
			}
			return sampleNames;
		}

		protected override void OnListItemClick ( ListView l, View v, int position, long id )
		{
			Intent myIntent = new Intent ( this, _samples[position] );
			this.StartActivity ( myIntent );
		}
	}
}

