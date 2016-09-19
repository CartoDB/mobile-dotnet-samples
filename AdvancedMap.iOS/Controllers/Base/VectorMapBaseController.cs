
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

namespace AdvancedMap.iOS
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

		Dictionary<string, string> styleDict = new Dictionary<string, string> {
			{ "Basic", "basic" },
			{ "NutiBright 2D", "nutibright-v3" },
			{ "NutiBright 3D", "nutibright3d" },
			{ "Loose Leaf", "looseleaf" }
		};

		Dictionary<string, string> languageDict = new Dictionary<string, string> {
			{ "English", "en" },
			{ "German", "de" },
			{ "Spanish", "es" },
			{ "Italian", "it" },
			{ "French", "fr" },
			{ "Russian", "ru" },
			{ "Chinese", "zh" }
		};

		OptionsMenu Menu { get; set; }
		MenuButton MenuButton { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			MapView.Options.ZoomRange = new MapRange(0, 20);

			UpdateBaseLayer();

			Menu = new OptionsMenu();
			Menu.AddItems("Style", styleDict, OptionSelectType.Style);
			Menu.AddItems("Language", languageDict, OptionSelectType.Language);

			Menu.SetInitialValueOf("Style", BaseStyle);
			Menu.SetInitialValueOf("Language", BaseLanguage);

			MenuButton = new MenuButton();
			NavigationItem.RightBarButtonItem = MenuButton;
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
				Alert("Map style file must be in project assets: ");
				return;
			}

			// Create style set
			CompiledStyleSet vectorTileStyleSet = new CompiledStyleSet(new ZippedAssetPackage(styleBytes));
			vectorTileDecoder = new MBVectorTileDecoder(vectorTileStyleSet);

			// Set language, language-specific texts from vector tiles will be used
			vectorTileDecoder.SetStyleParameter("lang", vectorStyleLang);

			// OSM Bright style set supports choosing between 2d/3d buildings. Set corresponding parameter.
			if (styleBuildings3D)
			{
				vectorTileDecoder.SetStyleParameter("buildings3d", "1");
			}

			vectorTileDecoder.SetStyleParameter("markers3d", "1");
			vectorTileDecoder.SetStyleParameter("texts3d", "1");
			vectorTileDecoder.SetStyleParameter("shields3d", "1");

			vectorTileDecoder.SetStyleParameter("contour_stroke", "rgba(217, 166, 140, 0.53)");
			vectorTileDecoder.SetStyleParameter("contour_width", "0.8");

			// Create tile data source for vector tiles
			if (vectorTileDataSource == null)
			{
				vectorTileDataSource = CreateTileDataSource();
			}

			// Remove old base layer, create new base layer
			if (BaseLayer != null)
			{
				MapView.Layers.Remove(BaseLayer);
			}

			BaseLayer = new VectorTileLayer(vectorTileDataSource, vectorTileDecoder);
			MapView.Layers.Insert(0, BaseLayer);
		}

		protected virtual TileDataSource CreateTileDataSource()
		{
			TileDataSource source = new CartoOnlineTileDataSource("nutiteq.osm");

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

