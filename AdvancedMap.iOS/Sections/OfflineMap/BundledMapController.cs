
using System;
using System.IO;
using System.Linq;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Projections;
using Carto.VectorTiles;
using Foundation;
using Shared;
using Shared.iOS;

namespace AdvancedMap.iOS
{
	public class BundledMapController : MapBaseController
	{
		public override string Name { get { return "Bundled Map"; } }

		public override string Description { get { return "Offline map of Rome bundled with the app"; } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Add default base layer
            AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);

			Projection projection = MapView.Options.BaseProjection;

			TileDataSource source = FileUtils.CreateTileDataSource("rome_carto-streets", "mbtiles");

			// Get decoder from current layer,
			// so we wouldn't need a style asset to create a decoder from scratch
			MBVectorTileDecoder decoder = (MBVectorTileDecoder)(MapView.Layers[0] as VectorTileLayer).TileDecoder;

			// Remove default baselayer
			MapView.Layers.Clear();

			// Add our new layer
			var layer = new VectorTileLayer(source, decoder);
			MapView.Layers.Insert(0, layer);

			// Zoom to the correct location
			MapPos rome = projection.FromWgs84(new MapPos(12.4807, 41.8962));
			MapView.SetFocusPos(rome, 0);
			MapView.SetZoom(13, 0);
		}

	}
}

