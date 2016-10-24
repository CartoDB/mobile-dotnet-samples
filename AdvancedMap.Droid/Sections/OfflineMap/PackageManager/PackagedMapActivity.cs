
using System;
using Android.App;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity]
	public class PackagedMapActivity : VectorBaseMapActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			MapView.SetZoom(3, 0);
		}

		protected override Carto.DataSources.TileDataSource CreateTileDataSource()
		{
			// Using static global variable to pass data. Bad style, avoid this pattern in your apps     
			if (PackageManagerActivity.DataSource != null)
			{
				return PackageManagerActivity.DataSource;
			}

			return base.CreateTileDataSource();
		}
	}
}

