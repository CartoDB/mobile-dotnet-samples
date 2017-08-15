
using System.IO;

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
    }
}
