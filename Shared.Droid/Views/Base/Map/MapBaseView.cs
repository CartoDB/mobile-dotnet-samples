
using System;
using System.Collections.Generic;
using Android.Content;
using Carto.Layers;
using Carto.Projections;
using Carto.Ui;

namespace Shared.Droid
{
    public class MapBaseView : BaseView
    {
        public Banner Banner { get; private set; }

		public MapView MapView { get; private set; }

		public SlideInPopup Popup { get; private set; }

		public Projection Projection
		{
			get { return MapView.Options.BaseProjection; }
		}

		protected int BottomLabelHeight
		{
			get { return (int)(40 * Density); }
		}

		protected int SmallPadding
		{
			get { return (int)(5 * Density); }
		}

		public ActionButton InfoButton { get; set; }

		public MapBaseView(Context context, int infoIcon, int backIcon, int closeIcon, int bannerIcon) : base(context)
        {
			Popup = new SlideInPopup(context, backIcon, closeIcon);
			AddView(Popup);

			MapView = new MapView(context);
            AddView(MapView);

            //InfoButton = new ActionButton(context, infoIcon);
            //AddButton(InfoButton);

            Banner = new Banner(context, bannerIcon);
            AddView(Banner);
        }

		public override void LayoutSubviews()
        {
            int x = 0;
            int y = 0;
            int w = Frame.W;
            int h = Frame.H;

            MapView.SetFrame(x, y, w, h);
            Popup.Frame = new CGRect(x, y, w, h);

			int count = buttons.Count;

            var buttonWidth = (int)(60 * Density);
            var innerPadding = (int)(25 * Density);
			var totalArea = buttonWidth * count + (innerPadding * (count - 1));

			w = buttonWidth;
			h = w;
            y = Frame.H - (BottomLabelHeight + h + SmallPadding);
            x = Frame.W / 2 - totalArea / 2;

            foreach (ActionButton button in buttons)
			{
				button.Frame = new CGRect(x, y, w, h);
				x += w + innerPadding;
			}

            Banner.Frame = new CGRect(0, 0, Frame.W, (int)(50 * Density));
        }

        readonly List<ActionButton> buttons = new List<ActionButton>();

        public void AddButton(ActionButton button)
        {
            buttons.Add(button);
            AddView(button);
        }

        public void RemoveButton(ActionButton button)
        {
            buttons.Remove(button);
            RemoveView(button);
        }

        public CartoOnlineVectorTileLayer AddBaseLayer(CartoBaseMapStyle style)
        {
            var layer = new CartoOnlineVectorTileLayer(style);
            MapView.Layers.Add(layer);
            return layer;
        }
    }
}
