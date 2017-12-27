
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
            else if (source.Equals(StylePopupContent.CartoRasterSource))
            {
				if (selection.Equals(StylePopupContent.HereSatelliteDaySource))
				{
					CurrentLayer = new CartoOnlineRasterTileLayer("here.satellite.day@2x");
				}
				else if (selection.Equals(StylePopupContent.HereNormalDaySource))
				{
					CurrentLayer = new CartoOnlineRasterTileLayer("here.normal.day@2x");
				}
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
