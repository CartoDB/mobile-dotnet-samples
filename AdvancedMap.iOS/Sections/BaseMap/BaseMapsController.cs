
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Utils;
using Shared;
using Shared.iOS;

namespace AdvancedMap.iOS
{
	public class BaseMapsController : MapBaseController
	{
		public override string Name { get { return "Choice of different Base Maps"; } }

		public override string Description { get { return "Overview of base maps offered by CARTO"; } }

		public OptionsMenu Menu { get; set; }
		MenuButton MenuButton { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Menu = new OptionsMenu();
			Menu.Items = Sections.List;

			MenuButton = new MenuButton();
			NavigationItem.RightBarButtonItem = MenuButton;

			Menu.SetInitialItem(Sections.List[0]);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			MenuButton.Click += OnMenuButtonClick;
			Menu.OptionTapped += OnMenuSelectionChanged;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			MenuButton.Click -= OnMenuButtonClick;
			Menu.OptionTapped -= OnMenuSelectionChanged;
		}

		void OnMenuButtonClick(object sender, EventArgs e)
		{
			if (Menu.IsVisible)
			{
				Menu.Hide();
			}
			else {
				Menu.Show();
			}
		}

		void OnMenuSelectionChanged(object sender, OptionEventArgs e)
		{
			UpdateBaseLayer(e.Section, e.Option.Value);
		}

		void UpdateBaseLayer(Section section, string selection)
		{
			if (section.Type == MapType.Vector)
			{
				CartoOnlineVectorTileLayer layer = null;
				string osm = section.OSM.Value;

				if (osm == "nutiteq.osm")
				{
					// Nutiteq styles are bundled with the SDK, we can initialize them via constuctor
					if (selection == "default")
					{
						layer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);
					}
					else if (selection == "gray")
					{
						layer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleGray);
					}
					else
					{
						layer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDark);
					}
				}
				else if (osm == "mapzen.osm")
				{
					// MapZen styles are not, styles need to manually added to assets and then decoded
					BinaryData styleAsset = AssetUtils.LoadAsset(selection + ".zip");
					layer = new CartoOnlineVectorTileLayer(osm, new ZippedAssetPackage(styleAsset));
				}

				MapView.Layers.Clear();
				MapView.Layers.Add(layer);
		    }
			else
			{
				// We know that the value of raster will be Positron or Darkmatter,
				// as Nutiteq and Mapzen use vector tiles
				string url = (selection == "positron") ? Urls.Positron : Urls.DarkMatter;

				TileDataSource source = new HTTPTileDataSource(1, 19, url);
				var layer = new RasterTileLayer(source);

				MapView.Layers.Clear();
				MapView.Layers.Add(layer);
			}

			Menu.Hide();
		}

	}
}

