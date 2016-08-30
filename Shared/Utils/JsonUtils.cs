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

	}
}

