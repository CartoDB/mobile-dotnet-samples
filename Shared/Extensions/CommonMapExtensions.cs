
using System;
using System.Collections.Generic;
using System.Linq;
using Carto.Core;
using Carto.PackageManager;

namespace Shared
{
	public static class CommonMapExtensions
	{
		public static List<Package> GetPackages(this PackageManager packageManager, string language, string folder)
		{
			List<Package> packages = new List<Package>();

			foreach (PackageInfo info in packageManager.ServerPackages)
			{
				StringVector names = info.GetNames(language);

				foreach (string name in names)
				{
					if (!name.StartsWith(folder))
					{
						continue; // belongs to a different folder, so ignore
					}

					string modified = name.Substring(folder.Length);
					int index = modified.IndexOf('/');
					Package package;

					if (index == -1)
					{
						// This is an actual package
						PackageStatus packageStatus = packageManager.GetLocalPackageStatus(info.PackageId, -1);
						package = new Package(modified, info, packageStatus);
					}
					else {
						// This is a package group
						modified = modified.Substring(0, index);
						if (packages.Any(i => i.Name == modified))
						{
							// Do not add if already contains
							continue;
						}
						package = new Package(modified, null, null);
					}

					packages.Add(package);
				}
			}

			return packages;

		}
	}
}

