using System;
using System.Json;

namespace Shared
{
	public class JsonUtils
	{
		public static JsonValue GetConfigJson(string sql, string cartoCSS)
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
	}
}

