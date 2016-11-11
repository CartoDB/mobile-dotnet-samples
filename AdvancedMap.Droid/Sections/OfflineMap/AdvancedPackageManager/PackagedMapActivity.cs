
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

			// Create style set
			PackageManagerTileDataSource source = MenuClickListener.DataSource;

			BinaryData styleBytes = AssetUtils.LoadAsset("nutiteq-dark.zip");
			var style = new CompiledStyleSet(new ZippedAssetPackage(styleBytes));

			// Create Decoder
			var decoder = new MBVectorTileDecoder(style);

			var layer = new VectorTileLayer(source, decoder);

			MapView.Layers.Add(layer);
		}

	}
}

