using System;
using System.Threading;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.VectorTiles;
using Shared;
using Shared.iOS;

namespace CartoMap.iOS
{
	public class CartoTorqueController : VectorMapBaseController
	{
		public override string Name { get { return "Carto Torque Map"; } }

		public override string Description { get { return "How to use Carto Torque tiles with CartoCSS styling"; } }

		const long FRAMETIME = 100;

		string CartoCSS
		{
			get
			{
				return "#layer {\n" +
					   "  comp-op: lighten;\n" +
					   "  marker-type:ellipse;\n" +
					   "  marker-width: 10;\n" +
					   "  marker-fill: #FEE391;\n" +
					   "  [value > 2] { marker-fill: #FEC44F; }\n" +
					   "  [value > 3] { marker-fill: #FE9929; }\n" +
					   "  [value > 4] { marker-fill: #EC7014; }\n" +
					   "  [value > 5] { marker-fill: #CC4C02; }\n" +
					   "  [value > 6] { marker-fill: #993404; }\n" +
					   "  [value > 7] { marker-fill: #662506; }\n" +
					   "\n" +
					   "  [frame-offset = 1] {\n" +
					   "    marker-width: 20;\n" +
					   "    marker-fill-opacity: 0.1;\n" +
					   "  }\n" +
					   "  [frame-offset = 2] {\n" +
					   "    marker-width: 30;\n" +
					   "    marker-fill-opacity: 0.05;\n" +
					   "  }\n" +
					   "}\n";
			}
		}

		// Magic query to create torque tiles
		string Query
		{
			get
			{
				return "WITH par \n" +
						"AS (SELECT Cdb_xyz_resolution({zoom}) * 1   AS res,\n" +
						"256 / 1 AS tile_size,\n" +
						"Cdb_xyz_extent({x}, {y}, {zoom}) AS ext),\n" +
						"cte\n" +
						"AS (SELECT St_snaptogrid(i.the_geom_webmercator, p.res) g,\n" +
						" Count(cartodb_id) c,\n" +
						" Floor(( Date_part('epoch', date) - -1796072400 ) / 476536.5) d\n" +
						"FROM (SELECT *\n" +
						" FROM ow) i,\n" +
						"  par p\n" +
						" WHERE i.the_geom_webmercator && p.ext\n" +
						" GROUP BY g, d)\n" +
						"SELECT ( St_x(g) - St_xmin(p.ext) ) / p.res x__uint8,\n" +
						" ( St_y(g) - St_ymin(p.ext) ) / p.res y__uint8,\n" +
						" Array_agg(c) vals__uint8,\n" +
						" Array_agg(d) dates__uint16\n" +
						"FROM cte,\n" +
						"  par p\n" +
						"WHERE  ( St_y(g) - St_ymin(p.ext) ) / p.res < tile_size\n" +
						" AND ( St_x(g) - St_xmin(p.ext) ) / p.res < tile_size\n" +
						"GROUP BY x__uint8,\n" +
						" y__uint8 ";
			}
		}

		TorqueTileDecoder decoder;
		TorqueTileLayer tileLayer;

		Timer timer;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			string encoded = System.Web.HttpUtility.UrlEncode(Query.Replace("\n", "")).EncodeParenthesis();

			// Define datasource with the query
			string url = "http://viz2.cartodb.com/api/v2/sql?q=" + encoded + "&cache_policy=persist";
			HTTPTileDataSource source = new HTTPTileDataSource(0, 14, url);

			// Create persistent cache to make it faster
			string cacheFile = Utils.GetDocumentDirectory() + "/torque_tile_cache.db";
			TileDataSource cacheSource = new PersistentCacheTileDataSource(source, cacheFile);

			// Create CartoCSS style from Torque points
			CartoCSSStyleSet styleSheet = new CartoCSSStyleSet(CartoCSS);

			// Create tile decoder and Torque layer
			decoder = new TorqueTileDecoder(styleSheet);

			tileLayer = new TorqueTileLayer(cacheSource, decoder);

			MapView.Layers.Add(tileLayer);

			MapView.SetZoom(1, 0);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			timer = new Timer(new TimerCallback(UpdateTorque), null, FRAMETIME, FRAMETIME);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

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

