
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

		public static List<Package> GetPackages(this PackageManager packageManager, string language, string folder)
		{
			List<Package> packages = new List<Package>();

			foreach (PackageInfo info in packageManager.ServerPackages)
			{
				StringVector names = info.GetNames(language);

				foreach (string name in names)
				{
					Console.WriteLine(name);
					if (!name.StartsWith(folder))
					{
						// Belongs to a different folder,
						// should not be added if name is e.g. Asia/, while folder is /Europe
						continue;
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
					else
					{
						// This is a package group
						modified = modified.Substring(0, index);

						// Try n' find an existing package from the list.
						List<Package> existing = packages.Where(i => i.Name == modified).ToList();

						if (existing.Count == 0)
						{
							// If there are none, add a package group if we don't have an existing list item
							package = new Package(modified, null, null);
						}
						else if (existing.Count == 1 && existing[0].Info != null)
						{
							// Sometimes we need to add two labels with the same name.
							// One a downloadable package and the other pointing to a list of said country's counties,
							// such as with Spain, Germany, France, Great Britain

							// If there is one existing package and its info isn't null,
							// we will add a "parent" package containing subpackages (or package group)
							package = new Package(modified, null, null);
						}
						else
						{
							// Shouldn't be added, as both cases are accounted for
							continue;
						}

					}

					packages.Add(package);
				}
			}

			return packages;
		}
		public static List<Package> GetPackages(this PackageManager packageManager, bool withRouting = false)
		{
			string language = "en";
			List<Package> packages = new List<Package>();

			foreach (PackageInfo info in packageManager.ServerPackages)
			{
				string name = info.GetNames(language)[0];
				string[] split = name.Split('/');

				// If package id contains -, then it's a subdivision (province, county, oblast etc.) of a country,
				// add the country as well as the subdivision.

				// Additionally check if it contains 'routing', as routing packages contain '-' are not subdivions
				if (info.PackageId.Contains("-") && split.Length > 2)
				{
					name = split[split.Length - 2] + ", " + split[split.Length - 1];
				}
				else {
					name = split[split.Length - 1];
				}

				Console.WriteLine("GetPackages: " + info.PackageId + " - " + name + " (" + info.GetNames(language).Count + ")");
				PackageStatus status = packageManager.GetLocalPackageStatus(info.PackageId, -1);
				var package = new Package(name, info, status);

				packages.Add(package);
			}

			// Order alphabetically
			packages = packages.OrderBy(package => package.Name).ToList();

			return packages;
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

		public static VectorTileListener InitializeVectorTileListener(this MapView map, VectorLayer vectorLayer)
		{
			LocalVectorDataSource source = new LocalVectorDataSource(map.Options.BaseProjection);
			vectorLayer = new VectorLayer(source);
			map.Layers.Add(vectorLayer);

			Layer layer = map.Layers[0];

			if (layer is VectorTileLayer)
			{
				var listener = new VectorTileListener(vectorLayer);
				(layer as VectorTileLayer).VectorTileEventListener = listener;
				return listener;
			}

			return null;
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
	}
}

