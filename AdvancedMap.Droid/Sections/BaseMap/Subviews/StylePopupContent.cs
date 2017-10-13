using System;
using System.Collections.Generic;
using AdvancedMap.Droid.Sections.BaseMap.Views;
using Android.Content;
using Android.Views;
using Android.Widget;
using Shared.Droid;

namespace AdvancedMap.Droid.Sections.BaseMap.Subviews
{
    public class StylePopupContent : BaseView
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

        public const string PositronUrl = "http://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png";
        public const string DarkMatterUrl = "http://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}.png";

        BaseView container;
        ScrollView scrollContainer;

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

        public StylePopupContent(Context context) : base(context)
        {
            scrollContainer = new ScrollView(context);
            AddView(scrollContainer);

            container = new BaseView(context);
            scrollContainer.AddView(container);

            CartoVector = new StylePopupContentSection(context);
            CartoVector.Source = CartoVectorSource;
            CartoVector.Header.Text = "CARTO VECTOR";
            CartoVector.AddItem(Voyager, Resource.Drawable.style_image_nutiteq_voyager);
            CartoVector.AddItem(Positron, Resource.Drawable.style_image_nutiteq_positron);
            CartoVector.AddItem(DarkMatter, Resource.Drawable.style_image_nutiteq_darkmatter);
            container.AddView(CartoVector);

            Mapzen = new StylePopupContentSection(context);
            Mapzen.Source = MapzenSource;
            Mapzen.Header.Text = "MAPZEN VECTOR";
            Mapzen.AddItem(Bright, Resource.Drawable.style_image_mapzen_bright);
            Mapzen.AddItem(Positron, Resource.Drawable.style_image_mapzen_positron);
            Mapzen.AddItem(DarkMatter, Resource.Drawable.style_image_mapzen_darkmatter);
            container.AddView(Mapzen);

            CartoRaster = new StylePopupContentSection(context);
            CartoRaster.Source = CartoRasterSource;
            CartoRaster.Header.Text = "HERE RASTER";
            CartoRaster.AddItem(HereSatelliteDaySource, Resource.Drawable.style_image_here_satellite);
            CartoRaster.AddItem(HereNormalDaySource, Resource.Drawable.style_image_here_normal);
            container.AddView(CartoRaster);
        }

        public override void LayoutSubviews()
        {
            int padding = (int)(5 * Density);
            int headerPadding = (int)(20 * Density);

            int x = padding;
            int y = 0;
            int w = Frame.W - 2 * padding;
            int h = CartoVector.CalculatedHeight;

            CartoVector.Frame = new CGRect(x, y, w, h);

            y += h + headerPadding;
            h = Mapzen.CalculatedHeight;

            Mapzen.Frame = new CGRect(x, y, w, h);

            y += h + headerPadding;
            h = CartoRaster.CalculatedHeight + headerPadding;

            CartoRaster.Frame = new CGRect(x, y, w, h);
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
