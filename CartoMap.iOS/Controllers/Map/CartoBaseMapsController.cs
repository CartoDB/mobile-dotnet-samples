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

		Dictionary<string, string> nutiteqStyles = new Dictionary<string, string> {
			{ "Default", "nutibright:default" },
			{ "Dark", "nutibright:dark" }
		};

		Dictionary<string, string> mapzenStyles = new Dictionary<string, string> {
			{ "Positron", "positron" },
			{ "Dark Matter", "dark_matter" }
		};

		Dictionary<string, string> selectedStyle;

		protected override Dictionary<string, string> GetStyleDict()
		{
			return selectedStyle;
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
			selectedStyle = nutiteqStyles;

			vectorStyleName = GetStyleDict()["Default"];
			vectorStyleTileType = GetTileTypeDict()["Vector"];

			base.ViewDidLoad();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			OSMChanged += OnVectorStyleChanged;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			OSMChanged -= OnVectorStyleChanged;
		}

		void OnVectorStyleChanged(object sender, EventArgs e)
		{
			string selection = (string)sender;

			if (selection == "nutiteq.osm")
			{
				selectedStyle = nutiteqStyles;
				vectorStyleName = GetStyleDict()["Default"];
			}
			else
			{
				selectedStyle = mapzenStyles;
				vectorStyleName = GetStyleDict()["Positron"];
			}

			Menu.UpdateItems(0, selectedStyle, OptionSelectType.Style);
			Menu.SetInitialValueOf("Style", vectorStyleName);
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
				string selection = Menu.GetSelectedValueOf("OSM");

				CartoOnlineVectorTileLayer layer = null;

				if (selection == "nutiteq.osm")
				{
					if (vectorStyleName.Split(':')[1] == "default")
					{
						layer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);
					}
					else
					{ 
						layer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDark);
					}
				}
				else
				{
					var styleAsset = AssetUtils.LoadAsset(vectorStyleName + ".zip");
					layer = new CartoOnlineVectorTileLayer(vectorStyleOSM, new ZippedAssetPackage(styleAsset));
				}

				MapView.Layers.Add(layer);
			}
		}
	}
}

