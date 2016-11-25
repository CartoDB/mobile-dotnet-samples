
using System;
using Android.App;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorTiles;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Packaged Map", Description = "Maps that are downloaded using PackageManager")]
	public class PackagedMapActivity : MapBaseActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			var layer = new CartoOfflineVectorTileLayer(MenuClickListener.Manager, CartoBaseMapStyle.CartoBasemapStyleDefault);
			MapView.Layers.Add(layer);
		}

	}
}

