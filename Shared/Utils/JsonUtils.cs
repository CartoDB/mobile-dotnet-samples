using System;
using System.Json;

namespace Shared
{
	public class JsonUtils
	{
		public static JsonValue GetRasterLayerConfigJson(string sql, string cartoCSS)
		{
			JsonObject configJson = new JsonObject();

			configJson.Add("asd", "asd");

			configJson.Add("version", "1.2.0");

			JsonArray layersArrayJson = new JsonArray();
			JsonObject layersJson = new JsonObject();

			layersJson.Add("type", "cartodb");

			JsonObject optionsJson = new JsonObject();

			optionsJson.Add("sql", sql);
			optionsJson.Add("cartocss", cartoCSS);
			optionsJson.Add("cartocss_version", "2.3.0");
			optionsJson.Add("geom_column", "the_raster_webmercator");
			optionsJson.Add("geom_type", "raster");

			layersJson.Add("options", optionsJson);

			layersArrayJson.Add(layersJson);

			configJson.Add("layers", layersArrayJson);

			return configJson;
		}


		static string Sql { get { return "select * from stations_1"; } }

		static string StatTag { get { return "3c6f224a-c6ad-11e5-b17e-0e98b61680bf"; } }

		static string[] Columns { get { return new string[] { "name", "field_9", "slot" }; } }

		static string CartoCSS
		{
			get
			{
				return "#stations_1{" +
							"marker-fill-opacity:0.9;" +
							"marker-line-color:#FFF;" +
							"marker-line-width:2;" +
							"marker-line-opacity:1;" +
							"marker-placement:point;" +
							"marker-type:ellipse;" +
							"marker-width:10;" +
							"marker-allow-overlap:true;" +
						"}\n" +
						"#stations_1[status = 'In Service']{marker-fill:#0F3B82;}\n" +
						"#stations_1[status = 'Not In Service']{marker-fill:#aaaaaa;}\n" +
						"#stations_1[field_9 = 200]{marker-width:80.0;}\n" +
						"#stations_1[field_9 <= 49]{marker-width:25.0;}\n" +
						"#stations_1[field_9 <= 38]{marker-width:22.8;}\n" +
						"#stations_1[field_9 <= 34]{marker-width:20.6;}\n" +
						"#stations_1[field_9 <= 29]{marker-width:18.3;}\n" +
						"#stations_1[field_9 <= 25]{marker-width:16.1;}\n" +
						"#stations_1[field_9 <= 20.5]{marker-width:13.9;}\n" +
						"#stations_1[field_9 <= 16]{marker-width:11.7;}\n" +
						"#stations_1[field_9 <= 12]{marker-width:9.4;}\n" +
						"#stations_1[field_9 <= 8]{marker-width:7.2;}\n" +
						"#stations_1[field_9 <= 4]{marker-width:5.0;}";
			}
		}

		public static JsonValue UTFGridConfigJson
		{
			get
			{
				JsonObject json = new JsonObject();

				json.Add("version", "1.0.1");
				json.Add("stat_tag", StatTag);

				JsonArray layers = new JsonArray();
				JsonObject layerJson = new JsonObject();

				layerJson.Add("type", "cartodb");

				JsonObject optionJson = new JsonObject();

				optionJson.Add("sql", Sql);
				optionJson.Add("cartocss", CartoCSS);
				optionJson.Add("cartocss_version", "2.1.1");

				JsonArray interactivityJson = new JsonArray();
				interactivityJson.Add("cartodb_id");

				optionJson.Add("interactivity", interactivityJson);

				JsonObject attributesJson = new JsonObject();
				attributesJson.Add("id", "cartodb_id");

				JsonArray columnJson = new JsonArray();

				foreach (string column in Columns)
				{
					columnJson.Add(column);
				}

				attributesJson.Add("columns", columnJson);
				optionJson.Add("attributes", attributesJson);
				layerJson.Add("options", optionJson);
				layers.Add(layerJson);

				json.Add("layers", layers);

				return json;
			}
		}

		public static string TorqueCartoCSS
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
		public static string TorqueQuery
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

		public static string GetTorqueQuery()
		{
			string encoded = 
				"WITH%20par%20AS%20(%20%20SELECT%20CDB_XYZ_Resolution({zoom})*1%20as%20res%2C%20%20256%2F1%" +
				"20as%20tile_size%2C%20CDB_XYZ_Extent({x}%2C%20{y}%2C%20{zoom})%20as%20ext%20)%2Ccte%20AS%2" +
				"0(%20%20%20SELECT%20ST_SnapToGrid(i.the_geom_webmercator%2C%20p.res)%20g%2C%20count" +
				"(cartodb_id)%20c%2C%20floor((date_part(%27epoch%27%2C%20date)%20-%20-1796072400)%2F476536.5)" +
				"%20d%20%20FROM%20(select%20*%20from%20ow)%20i%2C%20par%20p%20%20%20WHERE%20i" +
				".the_geom_webmercator%20%26%26%20p.ext%20%20%20GROUP%20BY%20g%2C%20d)%20" +
				"SELECT%20(st_x(g)-st_xmin(p.ext))%2Fp.res%20x__uint8%2C%20%20%20%20%20%20%20%20" +
				"(st_y(g)-st_ymin(p.ext))%2Fp.res%20y__uint8%2C%20array_agg(c)%20vals" +
				"__uint8%2C%20array_agg(d)%20dates__uint16%20" +
				"FROM%20cte%2C%20par%20p%20where%20(st_y(g)-st_ymin(p.ext))%2Fp.res%20%3C%20" +
				"tile_size%20and%20(st_x(g)-st_xmin(p.ext))%2Fp.res%20%3C%20tile_size%20GROUP%20BY%20" +
				"x__uint8%2C%20y__uint8&last_updated=1970-01-01T00%3A00%3A00.000Z";

			return encoded;
		}

	}
}

