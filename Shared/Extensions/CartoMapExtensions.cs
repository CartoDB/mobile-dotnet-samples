using System;
using System.Json;
using System.Threading;
using Carto.Core;
using Carto.DataSources;
using Carto.Geometry;
using Carto.Layers;
using Carto.Projections;
using Carto.Services;
using Carto.Styles;
using Carto.Ui;
using Carto.Utils;
using Carto.VectorElements;

namespace Shared
{
	public static class CartoMapExtensions
	{
		public static void ConfigureAnonymousVectorLayers(this MapView map, JsonValue config)
		{
			// Use the Maps service to configure layers. 
			// Note that this must be done in a separate thread on Android, 
			// as Maps API requires connecting to server which is not nice to do in main thread.

			System.Threading.Tasks.Task.Run(delegate
			{
				CartoMapsService service = new CartoMapsService();
				service.Username = "nutiteq";

				// Use VectorLayers
				service.DefaultVectorLayerMode = true;
				service.Interactive = true;

				LayerVector layers = service.BuildMap(Variant.FromString(config.ToString()));

				for (int i = 0; i < layers.Count; i++)
				{
					map.Layers.Add(layers[i]);
				}
			});
		}

		public static void ConfigureNamedVectorLayers(this MapView map, string name)
		{
			System.Threading.Tasks.Task.Run(delegate
			{
				CartoMapsService service = new CartoMapsService();
				service.Username = "nutiteq";

				// Use VectorLayers
				service.DefaultVectorLayerMode = true;

				LayerVector layers = service.BuildNamedMap(name, new StringVariantMap());

				for (int i = 0; i < layers.Count; i++)
				{
					map.Layers.Add(layers[i]);
				}
			});
		}

		public static void QueryFeatures(this MapView map, CartoSQLService service, LocalVectorDataSource source, PointStyle style, string query)
		{
			System.Threading.Tasks.Task.Run(delegate
			{
				FeatureCollection features = service.QueryFeatures(query, map.Options.BaseProjection);

				for (int i = 0; i < features.FeatureCount; i++)
				{
					Feature feature = features.GetFeature(i);

					PointGeometry geometry = (PointGeometry)feature.Geometry;

					var point = new Point(geometry, style);
					source.Add(point);
				}

			});	
		}
	}

}

