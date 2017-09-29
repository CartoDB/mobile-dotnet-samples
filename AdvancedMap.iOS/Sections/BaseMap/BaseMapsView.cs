
using System;
using AdvancedMap.iOS.Sections.BaseMap.Subviews;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorTiles;
using Shared;
using Shared.iOS;
using Shared.Model;

namespace AdvancedMap.iOS.Sections.BaseMap
{
    public class BaseMapsView : MapBaseView
    {
        public PopupButton BasemapButton;
		public StylePopupContent StyleContent;

		public PopupButton LanguageButton;
		public LanguagePopupContent LanguageContent;
		
        public TileLayer CurrentLayer { get; set; }

		public BaseMapsView()
        {
			BasemapButton = new PopupButton("icons/icon_basemap.png");
			AddButton(BasemapButton);
			StyleContent = new StylePopupContent();

			LanguageButton = new PopupButton("icons/icon_language.png");
			AddButton(LanguageButton);
			LanguageContent = new LanguagePopupContent();
        }

        public void UpdateBaseLayer(string selection, string source)
        {
            if (source.Equals(StylePopupContent.CartoVectorSource))
            {
                // Nutiteq styles are bundled with the SDK, we can initialize them via constuctor
                if (selection.Equals(StylePopupContent.Voyager))
                {
                    CurrentLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
                }
                else if (selection.Equals(StylePopupContent.Positron))
                {
                    CurrentLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStylePositron);
                }
                else
                {
                    CurrentLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDarkmatter);
                }
            }
            else if (source.Equals(StylePopupContent.MapzenSource))
            {
                // Mapzen styles are all bundled in one .zip file.
                // Selection contains both the style name and file name (cf. Sections.cs in Shared)

                // Create a style set from the file and style
                BinaryData styleAsset = AssetUtils.LoadAsset("styles/styles_mapzen.zip");
                var package = new ZippedAssetPackage(styleAsset);

                string name = "";

                if (selection.Equals(StylePopupContent.Bright))
                {
                    // The name of the actual style is "bright", as it's displayed in the UI,
                    // but in the style file it's a default "style", change the name
                    name = "style";    
                } 
                else if (selection.Equals(StylePopupContent.Positron))
                {
                    name = "positron";
                }
                else if (selection.Equals(StylePopupContent.DarkMatter))
                {
                    name = "positron_dark";
                }

                CompiledStyleSet styleSet = new CompiledStyleSet(package, name);

                // Create datasource and style decoder
                var ds = new CartoOnlineTileDataSource(source);
                var decoder = new MBVectorTileDecoder(styleSet);

                CurrentLayer = new VectorTileLayer(ds, decoder);

                //ContentView.Menu.LanguageChoiceEnabled = true;
                //ResetLanguage();

            }
            else if (source.Equals(StylePopupContent.CartoRasterSource))
            {
                // We know that the value of raster will be Positron or Darkmatter,
                // as Nutiteq and Mapzen use vector tiles

                // Additionally, raster tiles do not support language choice
                string url = (selection == StylePopupContent.Positron) ? Urls.Positron : Urls.DarkMatter;

                TileDataSource ds = new HTTPTileDataSource(1, 19, url);
                CurrentLayer = new RasterTileLayer(ds);

                // Language choice not enabled in raster tiles
                //ContentView.Menu.LanguageChoiceEnabled = false;
            }

            if (source.Equals(StylePopupContent.CartoRasterSource))
            {
                LanguageButton.Disable();
            }
            else
            {
                LanguageButton.Enable();
            }

            MapView.Layers.Clear();
            MapView.Layers.Add(CurrentLayer);

            InitializeVectorTileListener();
        }

        public void UpdateLanguage(Language language)
        {
            if (CurrentLayer is VectorTileLayer)
            {
                var decoder = (CurrentLayer as VectorTileLayer).TileDecoder as MBVectorTileDecoder;
                decoder.SetStyleParameter("lang", language.Value);
            }
        }

        public void InitializeVectorTileListener()
        {
            if (CurrentLayer is VectorTileLayer)
            {
                MapView.InitializeVectorTileListener(CurrentLayer as VectorTileLayer);
            }
        }
    }
}
