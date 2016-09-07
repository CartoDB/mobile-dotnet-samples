using System;
using System.Json;
using System.Threading;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Projections;
using Carto.Services;
using Carto.Ui;

namespace Shared
{
	public class CartoMapUtils
	{
		public static void ConfigureUTFGridLayers(MapView MapView, JsonValue config)
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

				try
				{
					LayerVector layers = service.BuildMap(Variant.FromString(config.ToString()));

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

		}
	}
}

