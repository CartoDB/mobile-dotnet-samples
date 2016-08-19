
using System;
using System.Collections.Generic;
using System.Linq;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Projections;
using Carto.Styles;
using Carto.Ui;
using Carto.VectorElements;

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

		public static void AddMarkerToPosition(this MapView map, MapPos position)
		{
			// Initialize a local vector data source
			Projection projection = map.Options.BaseProjection;
			LocalVectorDataSource datasource = new LocalVectorDataSource(projection);

			// Initialize a vector layer with the previous data source
			VectorLayer layer = new VectorLayer(datasource);

			// Add layer to map
			map.Layers.Add(layer);

			MarkerStyleBuilder builder = new MarkerStyleBuilder();
			builder.Size = 30;
			builder.Color = new Carto.Graphics.Color(0, 255, 0, 255);
			//builder.Color = new Carto.Graphics.Color(0xFF00FF00);

			// Set marker position and style
			position = projection.FromWgs84(position);
			MarkerStyle style = builder.BuildStyle();

			// Create marker and add it to the source
			Marker marker = new Marker(position, style);
			datasource.Add(marker);
		}
	}
}

