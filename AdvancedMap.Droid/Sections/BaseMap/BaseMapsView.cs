using System;
using AdvancedMap.Droid.Sections.BaseMap.Subviews;
using Android.Content;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorTiles;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid.Sections.BaseMap.Views
{
	public class BaseMapsView : MapBaseView
	{
        public ActionButton BasemapButton;
        public StylePopupContent StyleContent;

		public BaseMapsView(Context context) : base(context,
													Resource.Drawable.icon_info_blue,
													Resource.Drawable.icon_back_blue,
												    Resource.Drawable.icon_close)
		{
            BasemapButton = new ActionButton(context, Resource.Drawable.icon_basemap);
            AddButton(BasemapButton);

            StyleContent = new StylePopupContent(context);

            Frame = new CGRect(0, 0, Metrics.WidthPixels, UsableHeight);
		}

		string currentOSM;
		string currentSelection;
        public TileLayer CurrentLayer { get; set; }

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
                BinaryData styleAsset = AssetUtils.LoadAsset("styles_mapzen.zip");
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

            MapView.Layers.Clear();
            MapView.Layers.Add(CurrentLayer);

            InitializeVectorTileListener();
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
