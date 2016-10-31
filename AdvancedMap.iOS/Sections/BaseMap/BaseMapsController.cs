
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

		string currentOSM;
		string currentSelection;

		void UpdateBaseLayer(Section section, string selection)
		{
			currentOSM = section.OSM.Value;
			currentSelection = selection;

			if (section.Type == SectionType.Language)
			{
				return;
			}

			TileLayer layer = null;


			if (section.Type == SectionType.Vector)
			{

				if (currentOSM == "nutiteq.osm")
				{
					// Nutiteq styles are bundled with the SDK, we can initialize them via constuctor
					if (currentSelection == "default")
					{
						layer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);
					}
					else if (currentSelection == "gray")
					{
						layer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleGray);
					}
					else
					{
						layer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDark);
					}
				}
				else if (currentOSM == "mapzen.osm")
				{
					// MapZen styles are not, styles need to manually added to assets and then decoded
					BinaryData styleAsset = AssetUtils.LoadAsset(currentSelection + ".zip");
					layer = new CartoOnlineVectorTileLayer(currentOSM, new ZippedAssetPackage(styleAsset));
				}
		    }
			else
			{
				// We know that the value of raster will be Positron or Darkmatter,
				// as Nutiteq and Mapzen use vector tiles

				// Additionally, raster tiles do not support language choice
				string url = (currentSelection == "positron") ? Urls.Positron : Urls.DarkMatter;

				TileDataSource source = new HTTPTileDataSource(1, 19, url);
				layer = new RasterTileLayer(source);
			}

			MapView.Layers.Clear();
			MapView.Layers.Add(layer);

			Menu.Hide();
		}

	}
}

