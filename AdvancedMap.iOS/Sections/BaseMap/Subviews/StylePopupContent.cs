using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;

namespace AdvancedMap.iOS.Sections.BaseMap.Subviews
{
    public class StylePopupContent : UIView
    {
        public const string CartoVectorSource = "carto.streets";
		public const string MapzenSource = "mapzen.osm";
		public const string CartoRasterSource = "carto.osm";

		public const string Bright = "BRIGHT";
		public const string Gray = "GRAY";
		public const string Dark = "DARK";

		public const string Positron = "POSITRON";
		public const string DarkMatter = "DARKMATTER";
		public const string Voyager = "VOYAGER";

		public const string HereSatelliteDaySource = "SATELLITE DAY";
		public const string HereNormalDaySource = "NORMAL DAY";

		UIScrollView container;
		
		public StylePopupContentSection CartoVector { get; private set; }
		public StylePopupContentSection Mapzen { get; private set; }
		public StylePopupContentSection CartoRaster { get; private set; }

		public List<StylePopupContentSection> Sections
		{
			get
			{
				return new List<StylePopupContentSection>
				{
					CartoVector, Mapzen, CartoRaster
				};
			}
		}

		public StylePopupContent()
        {
			container = new UIScrollView();
			AddSubview(container);

            string basefolder = "basemaps/";

			CartoVector = new StylePopupContentSection();
			CartoVector.Source = CartoVectorSource;
			CartoVector.Header.Text = "CARTO VECTOR";
			CartoVector.AddItem(Voyager, basefolder + "style_image_nutiteq_voyager.png");
			CartoVector.AddItem(Positron, basefolder + "style_image_nutiteq_positron.png");
			CartoVector.AddItem(DarkMatter, basefolder + "style_image_nutiteq_darkmatter.png");
			container.AddSubview(CartoVector);

			Mapzen = new StylePopupContentSection();
			Mapzen.Source = MapzenSource;
			Mapzen.Header.Text = "MAPZEN VECTOR";
			Mapzen.AddItem(Bright, basefolder + "style_image_mapzen_bright.png");
			Mapzen.AddItem(Positron, basefolder + "style_image_mapzen_positron.png");
			Mapzen.AddItem(DarkMatter, basefolder + "style_image_mapzen_darkmatter.png");
			container.AddSubview(Mapzen);

			CartoRaster = new StylePopupContentSection();
			CartoRaster.Source = CartoRasterSource;
			CartoRaster.Header.Text = "HERE RASTER";
            CartoRaster.AddItem(HereSatelliteDaySource, basefolder + "style_image_here_satellite.png");
            CartoRaster.AddItem(HereNormalDaySource, basefolder + "style_image_here_normal.png");
			container.AddSubview(CartoRaster);
		}

		public override void LayoutSubviews()
		{
            container.Frame = Bounds;

            nfloat padding = 5;
			nfloat headerPadding = 20;

			nfloat x = padding;
			nfloat y = 0;
			nfloat w = Frame.Width - 2 * padding;
			nfloat h = CartoVector.CalculatedHeight;

			CartoVector.Frame = new CGRect(x, y, w, h);

			y += h + headerPadding;
			h = Mapzen.CalculatedHeight;

			Mapzen.Frame = new CGRect(x, y, w, h);

			y += h + headerPadding;
			h = CartoRaster.CalculatedHeight + headerPadding;

			CartoRaster.Frame = new CGRect(x, y, w, h);

            container.ContentSize = new CGSize(Bounds.Width, y + h + padding);
		}

		public StylePopupContentSectionItem Previous { get; set; }

		public void HighlightDefault()
		{
			var item = CartoVector.List[0];

			item.Highlight();
			Previous = item;
		}
    }
}
