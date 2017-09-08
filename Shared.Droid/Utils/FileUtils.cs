
using System.IO;
using Carto.DataSources;
using Carto.Utils;

namespace Shared.Droid
{
    public class FileUtils
    {
		public static string GetExternalDirectory(string withFolder = null)
		{
			string external = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            string directory = external;

            if (withFolder != null)
            {
                Path.Combine(external, withFolder + "/");   
            }

			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			if (withFolder == null)
			{
				return external;
			}

			return directory;
		}

		public static TileDataSource CreateTileDataSource(Android.Content.Context context, string fileName)
		{
			try
			{
				string directory = context.GetExternalFilesDir(null).ToString();
				string path = directory + "/" + fileName;

				context.Assets.CopyAssetToSDCard(fileName, path);
				Log.Debug("Copy done to " + path);

				MBTilesTileDataSource source = new MBTilesTileDataSource(0, 14, path);

				return new MemoryCacheTileDataSource(source);
			}
			catch (IOException e)
			{
				Log.Debug("MbTileFile cannot be copied: " + fileName);
                Log.Debug("Message" + e.Message);
			}

			return null;
		}
    }
}
