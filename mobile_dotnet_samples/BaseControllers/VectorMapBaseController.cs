
using System;
using System.Collections.Generic;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorTiles;
using Shared.iOS;
using UIKit;

namespace Shared.iOS
{
	public class VectorMapBaseController : MapBaseController
	{
		const string Menu_Language = "Language";
		const string Menu_Style = "Style";

		public const string BaseStyle = "nutibright-v3";
		public const string BaseStyleFile = BaseStyle + ".zip";

		const string BaseLanguage = "en";

		protected TileDataSource vectorTileDataSource;
		protected MBVectorTileDecoder vectorTileDecoder;

		// Style Parameters
		protected string vectorStyleName = BaseStyle; // default style name, each style has corresponding .zip asset
		protected string vectorStyleLang = BaseLanguage; // default map language
		protected string vectorStyleTileType = "raster";
		protected string vectorStyleOSM = "nutiteq.osm";
			
		protected virtual Dictionary<string, string> GetStyleDict()
		{
			return new Dictionary<string, string> {
				{ "Basic", "basic" },
				{ "NutiBright 2D", "nutibright-v3" },
				{ "NutiBright 3D", "nutibright3d" },
				{ "Loose Leaf", "looseleaf" }
			};
		}

		protected virtual Dictionary<string, string> GetLanguageDict()
		{
			return new Dictionary<string, string> {
				{ "English", "en" },
				{ "German", "de" },
				{ "Spanish", "es" },
				{ "Italian", "it" },
				{ "French", "fr" },
				{ "Russian", "ru" },
				{ "Chinese", "zh" }
			};
		}

		protected virtual Dictionary<string, string> GetTileTypeDict()
		{
			return new Dictionary<string, string>();
		}

		protected virtual Dictionary<string, string> GetOSMDict()
		{
			return new Dictionary<string, string>();
		}

		public OptionsMenu Menu { get; set; }
		MenuButton MenuButton { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			MapView.Options.ZoomRange = new MapRange(0, 20);

			Menu = new OptionsMenu();

			Dictionary<string, string> styles = GetStyleDict();

			if (styles.Count > 0)
			{
				Menu.AddItems("Style", styles, OptionSelectType.Style);
				Menu.SetInitialValueOf("Style", vectorStyleName);
			}

			Dictionary<string, string> languages = GetLanguageDict();

			if (languages.Count > 0)
			{
				Menu.AddItems("Language", languages, OptionSelectType.Language);
				Menu.SetInitialValueOf("Language", BaseLanguage);
			}

			Dictionary<string, string> tileTypes = GetTileTypeDict();

			if (tileTypes.Count > 0)
			{
				Menu.AddItems("Tile type", tileTypes, OptionSelectType.TileType);
				Menu.SetInitialValueOf("Tile type", vectorStyleTileType);
			}

			Dictionary<string, string> osms = GetOSMDict();

			if (osms.Count > 0)
			{
				Menu.AddItems("OSM", osms, OptionSelectType.OSM);
				Menu.SetInitialValueOf("OSM", vectorStyleOSM);
			}

			MenuButton = new MenuButton();
			NavigationItem.RightBarButtonItem = MenuButton;

			UpdateBaseLayer();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			MenuButton.Click += OnMenuButtonClick;
			Menu.SelectionChanged += OnMenuSelectionChanged;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			MenuButton.Click -= OnMenuButtonClick;
			Menu.SelectionChanged -= OnMenuSelectionChanged;
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

		void OnMenuSelectionChanged(object sender, EventArgs e)
		{
			OptionsSelect option = (OptionsSelect)sender;

			if (option.Type == OptionSelectType.Style)
			{
				vectorStyleName = option.Value;
			}
			else if (option.Type == OptionSelectType.Language)
			{
				vectorStyleLang = option.Value;
			}
			else if (option.Type == OptionSelectType.TileType) 
			{
				vectorStyleTileType = option.Value;
			}
			else if (option.Type == OptionSelectType.OSM) 
			{
				vectorStyleOSM = option.Value;
			}

			UpdateBaseLayer();
		}

		protected virtual void UpdateBaseLayer()
		{
			string styleAssetName = vectorStyleName + ".zip";
			bool styleBuildings3D = false;

			if (vectorStyleName.Equals("nutibright3d"))
			{
				styleAssetName = BaseStyleFile;
				styleBuildings3D = true;
			}

			BinaryData styleBytes = AssetUtils.LoadAsset(styleAssetName);

			if (styleBytes == null)
			{
				Alert("Map style (" + styleAssetName + ") file must be in project assets");
				return;
			}

			// Create style set
			CompiledStyleSet vectorTileStyleSet = new CompiledStyleSet(new ZippedAssetPackage(styleBytes));
			vectorTileDecoder = new MBVectorTileDecoder(vectorTileStyleSet);

			// Set language, language-specific texts from vector tiles will be used
			vectorTileDecoder.SetStyleParameter("lang", vectorStyleLang);

			// OSM Bright style set supports choosing between 2d/3d buildings. Set corresponding parameter.
			if (styleAssetName.Equals(BaseStyleFile))
			{
				vectorTileDecoder.SetStyleParameter("buildings3d", styleBuildings3D ? "1" : "0");
				vectorTileDecoder.SetStyleParameter("markers3d", styleBuildings3D ? "1" : "0");
				vectorTileDecoder.SetStyleParameter("texts3d", styleBuildings3D ? "1" : "0");
			}

			// Create tile data source for vector tiles
			if (vectorTileDataSource == null)
			{
				vectorTileDataSource = CreateTileDataSource(vectorStyleOSM);
			}

			// Remove old base layer, create new base layer
			if (BaseLayer != null)
			{
				MapView.Layers.Remove(BaseLayer);
			}

			BaseLayer = new VectorTileLayer(vectorTileDataSource, vectorTileDecoder);
			MapView.Layers.Insert(0, BaseLayer);
		}

		protected virtual TileDataSource CreateTileDataSource(string osm)
		{
			TileDataSource source = new CartoOnlineTileDataSource(osm);
			return new MemoryCacheTileDataSource(source);
		}
	}

	public class MenuButton : UIBarButtonItem
	{
		public EventHandler<EventArgs> Click;

		UIImageView image;

		public MenuButton()
		{
			image = new UIImageView();
			image.Image = UIImage.FromFile("icon_more.png");
			image.Frame = new CoreGraphics.CGRect(0, 10, 20, 30);
			CustomView = image;

			image.AddGestureRecognizer(new UITapGestureRecognizer(OnImageClick));
		}

		void OnImageClick()
		{
			if (Click != null)
			{
				Click(new object(), EventArgs.Empty);
			}
		}

	}
}

