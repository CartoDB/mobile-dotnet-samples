
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

		public override string Description { get { return "Bundle MBTiles file for offline base map"; } }

		public string SupportDirectory { get { return Utils.GetDocumentDirectory("packages"); } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Projection projection = MapView.Options.BaseProjection;

			TileDataSource source = CreateTileDataSource();

			// Get decoder from current layer,
			// so we wouldn't need a style asset to create a decoder from scratch
			MBVectorTileDecoder decoder = (MBVectorTileDecoder)(MapView.Layers[0] as VectorTileLayer).TileDecoder;

			//BinaryData styleBytes = Carto.Utils.AssetUtils.LoadAsset("positron.zip");
			//var vectorTileStyleSet = new Carto.Styles.CompiledStyleSet(new Carto.Utils.ZippedAssetPackage(styleBytes));
			//decoder = new MBVectorTileDecoder(vectorTileStyleSet);

			// Set language, language-specific texts from vector tiles will be used
			//decoder.SetStyleParameter("lang", "en");

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

		protected TileDataSource CreateTileDataSource()
		{
			string name = "rome_ntvt";
			string extension = "mbtiles";


			string packageDirectory = SupportDirectory;
			string fullWritePath = Path.Combine(packageDirectory, name + "." + extension);
			string resourceDirectory = NSBundle.MainBundle.PathForResource(name, extension);

			//if (!Directory.Exists(packageDirectory))
			//{
			//	Directory.CreateDirectory(packageDirectory);
			//	Console.WriteLine("Directory: Does not exist... Creating");
			//}
			//else
			//{
			//	Console.WriteLine("Directory: Exists");
			//}

			//try
			//{
			//	// Copy bundled tile data to file system so it can be imported by package manager
			//	using (var input = new FileStream(resourceDirectory, FileMode.Open, FileAccess.Read))
			//	{
			//		using (var output = new FileStream(fullWritePath, FileMode.Create, FileAccess.Write))
			//		{
			//			input.CopyTo(output);
			//		}
			//	}

			//	return new MBTilesTileDataSource(0, 4, fullWritePath);
			//}
			//catch
			//{
			//	return null;
			//}

			return new MBTilesTileDataSource(0, 4, resourceDirectory);
		}
	}
}

