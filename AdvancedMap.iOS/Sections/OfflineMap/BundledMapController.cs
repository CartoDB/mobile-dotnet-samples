
using System;
using System.IO;
using System.Linq;
using Carto.Core;
using Carto.DataSources;
using Carto.Projections;
using Foundation;
using Shared;
using Shared.iOS;

namespace AdvancedMap.iOS
{
	// Formerly known as OfflineVectorMapController
	public class BundledMBTilesController : VectorMapBaseController
	{
		public override string Name { get { return "Bundled MBTiles"; } }

		public override string Description { get { return "Bundle MBTiles file for offline base map"; } }

		public string SupportDirectory { get { return Utils.GetDocumentDirectory("packages"); } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Projection projection = MapView.Options.BaseProjection;
			MapPos rome = projection.FromWgs84(new MapPos(12.4807, 41.8962));

			MapView.SetFocusPos(rome, 0);
			MapView.SetZoom(13, 0);
		}

		protected override TileDataSource CreateTileDataSource(string osm)
		{
			//string fileName = "world_zoom5";
			string fileName = "rome_ntvt";
			string extension = "mbtiles";

			string packageDirectory = SupportDirectory;
			string fullWritePath = Path.Combine(packageDirectory, fileName + "." + extension);

			string resourceDirectory = NSBundle.MainBundle.PathForResource(fileName, extension);

			return new MBTilesTileDataSource(0, 4, resourceDirectory);

			if (!Directory.Exists(packageDirectory))
			{
				Directory.CreateDirectory(packageDirectory);
				Console.WriteLine("Directory: Does not exist... Creating");
			}
			else
			{
				Console.WriteLine("Directory: Exists");
			}

			try
			{
				// Copy bundled tile data to file system so it can be imported by package manager
				using (var input = new FileStream(resourceDirectory, FileMode.Open, FileAccess.Read))
				{
					using (var output = new FileStream(fullWritePath, FileMode.Create, FileAccess.Write))
					{
						input.CopyTo(output);
					}
				}

				MBTilesTileDataSource source = new MBTilesTileDataSource(0, 4, fullWritePath);

				return source;
			}
			catch {
				return null;
			}
		}
	}
}

