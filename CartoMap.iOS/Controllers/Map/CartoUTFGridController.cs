using System;
using System.Json;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Services;
using Shared;

namespace CartoMap.iOS
{
	public class CartoUTFGridController : VectorMapBaseController
	{
		public override string Name { get { return "Carto UTF Grid"; } }

		public override string Description { 
			get { 
				return "A sample demonstrating how to use Carto Maps API with Raster tiles and UTFGrid"; 
			} 
		}

		string Sql { get { return "select * from stations_1"; } }

		string StatTag { get { return "3c6f224a-c6ad-11e5-b17e-0e98b61680bf"; } }

		string[] Columns { get { return new string[] { "name", "field_9", "slot" }; } }

		string CartoCSS
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

		JsonValue ConfigJson
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

				Console.WriteLine(json.ToString());
				return json;
			}
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Use the Maps service to configure layers. 
			// Note that this must be done in a separate thread on Android, 
			// as Maps API requires connecting to server which is not nice to do in main thread.
			System.Threading.Tasks.Task.Run(delegate
			{
				CartoMapsService service = new CartoMapsService();
				service.Username = "nutiteq";
				// Use VectorLayers
				service.DefaultVectorLayerMode = true;

				try
				{
					LayerVector layers = service.BuildMap(Variant.FromString(ConfigJson.ToString()));

					LocalVectorDataSource vectorDataSource = new LocalVectorDataSource(MapView.Options.BaseProjection);
					VectorLayer vectorLayer = new VectorLayer(vectorDataSource);

					for (int i = 0; i < layers.Count; i++)
					{
						TileLayer layer = (TileLayer)layers[i];
						TileDataSource ds = layer.UTFGridDataSource;
						MyUTFGridEventListener mapListener = new MyUTFGridEventListener(vectorDataSource);

						layer.UTFGridEventListener = mapListener;
						MapView.Layers.Add(layer);
					}

					MapView.Layers.Add(vectorLayer);
				}
				catch (Exception e)
				{
					Carto.Utils.Log.Debug("UTFGrid Exception: " + e.Message);
				}

			});

			// Animate map to the content area
			MapPos newYork = MapView.Options.BaseProjection.FromWgs84(new MapPos(-74.0059, 40.7127));
			MapView.SetFocusPos(newYork, 1);
			MapView.SetZoom(15, 1);
		}
	}
}

