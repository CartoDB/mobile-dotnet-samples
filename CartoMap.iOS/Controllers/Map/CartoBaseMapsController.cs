using System;
using System.Collections.Generic;
using Carto.Core;
using Carto.Layers;
using Carto.Styles;
using Carto.Utils;

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
				{ "Dark Matter", "darkmatter" }
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
			//base.UpdateBaseLayer();

			MapView.Layers.Clear();

			var styleAsset = AssetUtils.LoadAsset(vectorStyleName + ".zip");
			var baseLayer = new CartoOnlineVectorTileLayer(vectorStyleOSM, new ZippedAssetPackage(styleAsset));

			MapView.Layers.Add(baseLayer);
		}
	}
}

