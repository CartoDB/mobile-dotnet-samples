using System;
using AdvancedMap.Droid.Sections.BaseMap.Views;
using Android.Content;
using Android.Views;
using Android.Widget;
using Shared.Droid;

namespace AdvancedMap.Droid.Sections.BaseMap.Subviews
{
    public class StylePopupContent : BaseView
    {
        const string CartoVectorSource = "carto.streets";
        const string MapzenSource = "mapzen.osm";
        const string CartoRasterSource = "carto.osm";

        const string Bright = "BRIGHT";
        const string Gray = "GRAY";
        const string Dark = "DARK";

        const string Positron = "POSITRON";
        const string DarkMatter = "DARKMATTER";
        const string Voyager = "VOYAGER";

        const string PositronUrl = "http://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png";
        const string DarkMatterUrl = "http://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}.png";

        BaseView container;
        ScrollView scrollContainer;

        public StylePopupContentSection CartoVector { get; private set; }
        public StylePopupContentSection Mapzen { get; private set; }
        public StylePopupContentSection CartoRaster { get; private set; }

        public StylePopupContent(Context context) : base(context)
        {
            AddView(scrollContainer);
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
            CartoRaster.Header.Text = "CARTO RASTER";
            CartoRaster.AddItem(Positron, Resource.Drawable.style_image_carto_positron);
            CartoRaster.AddItem(DarkMatter, Resource.Drawable.style_image_carto_darkmatter);
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

            CartoVector.SetFrame(x, y, w, h);

            y += h + headerPadding;
            h = Mapzen.CalculatedHeight;

            Mapzen.SetFrame(x, y, w, h);

            y += h + headerPadding;
            h = CartoRaster.CalculatedHeight + headerPadding;

            CartoRaster.SetFrame(x, y, w, h);
        }

        public EventHandler<EventArgs> ItemClicked;

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.Action != MotionEventActions.Up)
            {
                return base.OnTouchEvent(e);
            }

            int x = (int)e.GetX();
            int y = (int)e.GetY();

            if (CartoVector.HitRect.Contains(x, y))
            {
                StylePopupContentSectionItem item = CartoVector.FindClickedItem(x, y);

                if (item != null)
                {
                    ItemClicked?.Invoke(item, EventArgs.Empty);
                }
            }
            else if (Mapzen.HitRect.Contains(x, y))
            {
                StylePopupContentSectionItem item = Mapzen.FindClickedItem(x, y);

				if (item != null)
				{
					ItemClicked?.Invoke(item, EventArgs.Empty);
				}
            }
            else if (CartoRaster.HitRect.Contains(x, y))
            {
                StylePopupContentSectionItem item = CartoRaster.FindClickedItem(x, y);

				if (item != null)
				{
					ItemClicked?.Invoke(item, EventArgs.Empty);
				}
            }

            return base.OnTouchEvent(e);
        }

        StylePopupContentSectionItem previous;

        public void HighlightDefault()
        {
            var item = CartoVector.List[0];

            item.Highlight();
            previous = item;
        }
    }
}
