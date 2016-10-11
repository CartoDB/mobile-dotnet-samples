using System;
using System.Collections.Generic;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Utils;
using Shared;
using Shared.iOS;

namespace CartoMap.iOS
{
	public class CartoBaseMapsController : VectorMapBaseController
	{
		public override string Name { get { return "Carto Base Maps"; } }

		public override string Description
		{
			get
			{
				return "Overview of base maps offered by CARTO";
			}
		}

		protected override Dictionary<string, string> GetStyleDict()
		{
			return new Dictionary<string, string> { 
				{ "Positron", "positron" }, 
				{ "Dark Matter", "dark_matter" }
			};
		}

		protected override Dictionary<string, string> GetLanguageDict()
		{
			return new Dictionary<string, string>();
		}

		protected override Dictionary<string, string> GetOSMDict()
		{
			return new Dictionary<string, string> {
				{ "Nutiteq", "nutiteq.osm" },
				{ "Mapzen", "mapzen.osm" }
			};
		}

		protected override Dictionary<string, string> GetTileTypeDict()
		{
			return new Dictionary<string, string> {
				{ "Raster", "raster" },
				{ "Vector", "vector" }
			};
		}

		public override void ViewDidLoad()
		{
			vectorStyleName = GetStyleDict()["Positron"];

			base.ViewDidLoad();
		}

		protected override void UpdateBaseLayer()
		{
			MapView.Layers.Clear();

			if (vectorStyleTileType == "raster")
			{
				Menu.Disable("OSM");

				string url = (vectorStyleName == "positron") ? Urls.Positron : Urls.DarkMatter;

				TileDataSource source = new HTTPTileDataSource(1, 19, url);
				var layer = new RasterTileLayer(source);

				MapView.Layers.Add(layer);
			}
			else 
			{
				Menu.Enable("OSM");

				var styleAsset = AssetUtils.LoadAsset(vectorStyleName + ".zip");
				var layer = new CartoOnlineVectorTileLayer(vectorStyleOSM, new ZippedAssetPackage(styleAsset));

				MapView.Layers.Add(layer);
			}
		}
	}
}

