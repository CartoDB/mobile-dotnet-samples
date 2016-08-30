
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
	[ActivityDescription(Description = "How to use Carto Torque tiles with CartoCSS styling")]
	public class CartoTorqueActivity : VectorBaseMapActivity
	{
		const long FRAMETIME = 100;

		TorqueTileDecoder decoder;
		TorqueTileLayer tileLayer;

		Timer timer;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			string encoded = System.Web.HttpUtility.UrlEncode(JsonUtils.TorqueQuery.Replace("\n", "")).EncodeParenthesis();

			encoded = "WITH%20par%20AS%20(%20%20SELECT%20CDB_XYZ_Resolution({zoom})*1%20as%20res%2C%20%20256%2F1%2" +
				"0as%20tile_size%2C%20CDB_XYZ_Extent({x}%2C%20{y}%2C%20{zoom})%20as%20ext%20)%2Ccte%20AS%20(%20%20" +
				"%20SELECT%20ST_SnapToGrid(i.the_geom_webmercator%2C%20p.res)%20g%2C%20count(cartodb_id)%20c%2C%20" +
				"floor((date_part(%27epoch%27%2C%20date)%20-%20-1796072400)%2F476536.5)%20d%20%20FROM%20(select%20" +
				"*%20from%20ow)%20i%2C%20par%20p%20%20%20WHERE%20i.the_geom_webmercator%20%26%26%20p.ext%20%20%20G" +
				"ROUP%20BY%20g%2C%20d)%20SELECT%20(st_x(g)-st_xmin(p.ext))%2Fp.res%20x__uint8%2C%20%20%20%20%20%20" +
				"%20%20(st_y(g)-st_ymin(p.ext))%2Fp.res%20y__uint8%2C%20array_agg(c)%20vals__uint8%2C%20array_agg" +
				"(d)%20dates__uint16%20FROM%20cte%2C%20par%20p%20where%20(st_y(g)-st_ymin(p.ext))%2Fp.res%20%3C%2" +
				"0tile_size%20and%20(st_x(g)-st_xmin(p.ext))%2Fp.res%20%3C%20tile_size%20GROUP%20BY%20x__uint8%2" +
				"C%20y__uint8&last_updated=1970-01-01T00%3A00%3A00.000Z";
			
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

