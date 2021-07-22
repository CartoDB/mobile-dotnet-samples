using System;
using System.IO;
using Carto.DataSources;
using Foundation;

namespace Shared.iOS
{
    public class FileUtils
    {
		public static string SupportDirectory { get { return Utils.GetDocumentDirectory("packages"); } }

		public static TileDataSource CreateTileDataSource(string name, string extension)
		{
			string packageDirectory = SupportDirectory;
			string fullWritePath = Path.Combine(packageDirectory, name + "." + extension);
			string resourceDirectory = NSBundle.MainBundle.PathForResource("mbtiles/" + name, extension);

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
				// Copy bundled tile data to file system
				using (var input = new FileStream(resourceDirectory, FileMode.Open, FileAccess.Read))
				{
					using (var output = new FileStream(fullWritePath, FileMode.Create, FileAccess.Write))
					{
						input.CopyTo(output);
					}
				}

				return new MBTilesTileDataSource(0, 14, fullWritePath);
			}
			catch
			{
				return null;
			}
		}
	}
}
