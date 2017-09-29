
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Projections;
using Carto.Services;
using Carto.Styles;
using Carto.Ui;
using Carto.Utils;
using Carto.VectorElements;

namespace Shared
{
	public static class CommonMapExtensions
	{
		public static void AddOnlineBaseLayer(this MapView map, CartoBaseMapStyle style)
		{
			var layer = new CartoOnlineVectorTileLayer(style);
			map.Layers.Add(layer);
		}

		public static Marker AddMarkerToPosition(this MapView map, MapPos position)
		{
			// Initialize a local vector data source
			Projection projection = map.Options.BaseProjection;
			LocalVectorDataSource datasource = new LocalVectorDataSource(projection);

			// Initialize a vector layer with the previous data source
			VectorLayer layer = new VectorLayer(datasource);

			// Add layer to map
			map.Layers.Add(layer);

			// Set marker style
			MarkerStyleBuilder builder = new MarkerStyleBuilder();
			builder.Size = 20;
			builder.Color = new Carto.Graphics.Color(0, 255, 0, 255);

			MarkerStyle style = builder.BuildStyle();

			// Create marker and add it to the source
			Marker marker = new Marker(position, style);
			datasource.Add(marker);

			return marker;
		}

		public static void UpdateVisWithGridEvent(this MapView map, string url, Action<string> error = null)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				map.Layers.Clear();

				// Create overlay layer for Popups
				Projection projection = map.Options.BaseProjection;
				LocalVectorDataSource source = new LocalVectorDataSource(projection);
				VectorLayer layer = new VectorLayer(source);

				// Create VIS loader
				CartoVisLoader loader = new CartoVisLoader();
                loader.DefaultVectorLayerMode = true;
				CartoVisBuilderWithGridEvent builder = new CartoVisBuilderWithGridEvent(map, layer);

				BinaryData fontData = AssetUtils.LoadAsset("carto-fonts.zip");
				loader.VectorTileAssetPackage = new ZippedAssetPackage(fontData);

				try
				{
					loader.LoadVis(builder, url);
				}
				catch (Exception e)
				{
					if (error != null)
					{
						error(e.Message);
					}
				}

				map.Layers.Add(layer);
			});
		}

		public static void AnimateZoomTo(this MapView map, MapPos position)
		{
			position = map.Options.BaseProjection.FromWgs84(new MapPos(24.650415, 59.428773));
			map.SetFocusPos(position, 2);
			map.Zoom = 14;
		}

        public static void InitializeVectorTileListener(this MapView map, VectorTileLayer layer)
		{
			LocalVectorDataSource source = new LocalVectorDataSource(map.Options.BaseProjection);
            VectorLayer popupLayer = new VectorLayer(source);
			map.Layers.Add(popupLayer);

			var listener = new VectorTileListener(popupLayer);
			(layer as VectorTileLayer).VectorTileEventListener = listener;
		}

		public static void InitializeVectorTileListener(this VectorTileLayer tileLayer, MapView map)
		{
			LocalVectorDataSource source = new LocalVectorDataSource(map.Options.BaseProjection);
			var vectorLayer = map.FindVectorLayer();

			if (vectorLayer == null)
			{
				vectorLayer = new VectorLayer(source);
				map.Layers.Add(vectorLayer);
			}

			tileLayer.VectorTileEventListener = new VectorTileListener(vectorLayer);
		}

		public static TileLayer FindTileLayer(this MapView map)
		{
			for (int i = 0; i < map.Layers.Count; i++)
			{
				var layer = map.Layers[i];

				if (layer is TileLayer)
				{
					return layer as TileLayer;
				}

			}

			return null;
		}

		public static VectorLayer FindVectorLayer(this MapView map)
		{
			for (int i = 0; i < map.Layers.Count; i++)
			{
				var layer = map.Layers[i];

				if (layer is VectorLayer)
				{
					return layer as VectorLayer;
				}

			}

			return null;
		}
	}
}

