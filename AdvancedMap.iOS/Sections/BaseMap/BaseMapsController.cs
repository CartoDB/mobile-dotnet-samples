
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorTiles;
using CoreGraphics;
using Shared;
using Shared.iOS;

namespace AdvancedMap.iOS
{
	public class BaseMapsController : MapBaseController
	{
		public override string Name { get { return "Choice of different Base Maps"; } }

		public override string Description { get { return "Overview of base maps offered by CARTO"; } }

		public BaseMapSectionMenu Menu { get; set; }
		MenuButton MenuButton { get; set; }

		VectorLayer VectorLayer { get; set; }

		ForceTouchRecognizer recognizer = new ForceTouchRecognizer();

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Menu = new BaseMapSectionMenu();
			Menu.Items = Sections.List;

			MenuButton = new MenuButton("icons/icon_more.png", new CGRect(0, 10, 20, 30));
			NavigationItem.RightBarButtonItem = MenuButton;

			// Set initial style 
			Menu.SetInitialItem(Sections.Nutiteq);
			Menu.SetInitialItem(Sections.Language);

			UpdateBaseLayer(Sections.Nutiteq, Sections.BaseStyleValue);
			UpdateLanguage(Sections.BaseLanguageCode);

			// Zoom to Central Europe so some texts would be visible
			MapPos europe = BaseProjection.FromWgs84(new MapPos(15.2551, 54.5260));
			MapView.SetFocusPos(europe, 0);
			MapView.Zoom = 5;

			recognizer = new ForceTouchRecognizer();
			MapView.AddGestureRecognizer(recognizer);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			MenuButton.Click += OnMenuButtonClick;
			Menu.OptionTapped += OnMenuSelectionChanged;

			recognizer.ForceTouch += OnForceTouch;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			MenuButton.Click -= OnMenuButtonClick;
			Menu.OptionTapped -= OnMenuSelectionChanged;

			recognizer.ForceTouch -= OnForceTouch;
		}

		void OnForceTouch(object sender, ForceEventArgs e)
		{
			currentListener.Force = e.RoundedForce;
			currentListener.IsForce = e.IsForce;
		}

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            Menu.Frame = View.Bounds;
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
		TileLayer currentLayer;

		VectorTileListener currentListener;

		void UpdateBaseLayer(Section section, string selection)
		{
			if (section.Type != SectionType.Language)
			{
				currentOSM = section.OSM.Value;
				currentSelection = selection;
			}

			if (section.Type == SectionType.Vector)
			{
                if (currentOSM == Sources.CartoVector)
				{
					// Nutiteq styles are bundled with the SDK, we can initialize them via constuctor
					if (currentSelection == "voyager")
					{
                        currentLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
					}
					else if (currentSelection == "positron")
					{
                        currentLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStylePositron);
					}
					else
					{
                        currentLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDarkmatter);
					}
				}
                else if (currentOSM == Sources.Mapzen)
				{
					// Mapzen styles are all bundled in one .zip file.
					// Selection contains both the style name and file name (cf. Sections.cs in Shared)
					string fileName = currentSelection.Split(':')[0];
					string styleName = currentSelection.Split(':')[1];

					// Create a style set from the file and style
					BinaryData styleAsset = AssetUtils.LoadAsset("styles/" + fileName + ".zip");
					var package = new ZippedAssetPackage(styleAsset);
					CompiledStyleSet styleSet = new CompiledStyleSet(package, styleName);

					// Create datasource and style decoder
					var source = new CartoOnlineTileDataSource(currentOSM);
					var decoder = new MBVectorTileDecoder(styleSet);

					currentLayer = new VectorTileLayer(source, decoder);
				
				}
				Menu.LanguageChoiceEnabled = true;
				ResetLanguage();

		    }
			else if (section.Type == SectionType.Raster)
			{
				// We know that the value of raster will be Positron or Darkmatter,
				// as Nutiteq and Mapzen use vector tiles

				// Additionally, raster tiles do not support language choice
				string url = (currentSelection == "positron") ? Urls.Positron : Urls.DarkMatter;

				TileDataSource source = new HTTPTileDataSource(1, 19, url);
				currentLayer = new RasterTileLayer(source);

				// Language choice not enabled in raster tiles
				Menu.LanguageChoiceEnabled = false;
			} 
			else if (section.Type == SectionType.Language)
			{
				if (currentLayer is RasterTileLayer) {
					// Raster tile language chance is not supported
					return;
				}
				UpdateLanguage(selection);
			}

			MapView.Layers.Clear();
			MapView.Layers.Add(currentLayer);

			Menu.Hide();

			currentListener = null;
			currentListener = MapView.InitializeVectorTileListener(VectorLayer);
		}

		void ResetLanguage()
		{
			Menu.SetInitialItem(Sections.Language);
			UpdateLanguage(Sections.BaseLanguageCode);
		}

		void UpdateLanguage(string code)
		{
			if (currentLayer == null) 
			{
				return;
			}

			MBVectorTileDecoder decoder = (currentLayer as VectorTileLayer).TileDecoder as MBVectorTileDecoder;
			decoder.SetStyleParameter("lang", code);
		}
	}
}

