
using System;
using System.Threading;
using Android.App;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.VectorTiles;
using Java.IO;
using Shared;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity]
	[ActivityData(Title = "Torque Ships", Description = "Animated ship movement in WWII")]
	public class TorqueShipsActivity : MapBaseActivity
	{
		const long FRAMETIME = 100;

		TorqueTileDecoder decoder;
		TorqueTileLayer tileLayer;

		Timer timer;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			string encoded = JsonUtils.GetTorqueQuery();

			// Define datasource with the query
			string url = "http://viz2.cartodb.com/api/v2/sql?q=" + encoded + "&cache_policy=persist";
			HTTPTileDataSource source = new HTTPTileDataSource(0, 14, url);

			// Create persistent cache to make it faster
			string cacheFile = GetExternalFilesDir(null) + "torque_tile_cache.db";
			TileDataSource cacheSource = new PersistentCacheTileDataSource(source, cacheFile);

			// Create CartoCSS style from Torque points
			CartoCSSStyleSet styleSheet = new CartoCSSStyleSet(JsonUtils.TorqueCartoCSS);

			// Create tile decoder and Torque layer
			decoder = new TorqueTileDecoder(styleSheet);

			tileLayer = new TorqueTileLayer(cacheSource, decoder);

			MapView.Layers.Add(tileLayer);

			MapView.SetZoom(1, 0);
		}

		protected override void OnStart()
		{
			base.OnStart();

			timer = new Timer(new TimerCallback(UpdateTorque), null, FRAMETIME, FRAMETIME);
		}

		protected override void OnStop()
		{
			base.OnStop();

			timer.Dispose();
			timer = null;
		}

		void UpdateTorque(object state)
		{
			System.Threading.Tasks.Task.Run(delegate
			{
				int frameNumber = (tileLayer.FrameNr + 1) % decoder.FrameCount;
				tileLayer.FrameNr = frameNumber;
			});
		}
	}
}

