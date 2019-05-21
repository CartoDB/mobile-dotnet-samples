using System;
using System.Collections.Generic;
using Carto.Layers;
using Carto.Projections;
using Carto.Ui;
using CoreGraphics;
using Shared.iOS.Views;
using UIKit;

namespace Shared.iOS
{
    public class MapBaseView : UIView
    {
        public Banner Banner { get; private set; }

		public MapView MapView { get; private set; }

		public SlideInPopup Popup { get; private set; }

		public Projection Projection
		{
			get { return MapView.Options.BaseProjection; }
		}

		public MapBaseView()
        {
			MapView = new MapView();
			AddSubview(MapView);

            MapView.Options.ZoomGestures = true;
			
            Banner = new Banner();
			AddSubview(Banner);

			Popup = new SlideInPopup();
			AddSubview(Popup);
			SendSubviewToBack(Popup);
		}

        protected nfloat bottomLabelHeight = 40;
        protected nfloat smallPadding = 5;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

			MapView.Frame = Bounds;
			Popup.Frame = Bounds;

			int count = buttons.Count;

			nfloat buttonWidth = 60;
			nfloat innerPadding = 25;
			nfloat totalArea = buttonWidth * count + (innerPadding * (count - 1));

			var w = buttonWidth;
			var h = w;
			var y = Frame.Height - (bottomLabelHeight + h + smallPadding);
			var x = Frame.Width / 2 - totalArea / 2;

			foreach (PopupButton button in buttons)
			{
				button.Frame = new CGRect(x, y, w, h);
				x += w + innerPadding;
			}

            Banner.Frame = new CGRect(0, Device.TrueY0, Frame.Width, 45);
		}

        readonly List<PopupButton> buttons = new List<PopupButton>();

        public void AddButton(PopupButton button)
        {
            buttons.Add(button);
            AddSubview(button);
        }

        public void RemoveButton(PopupButton button)
        {
            buttons.Remove(button);
            button.RemoveFromSuperview();
        }

        public CartoOnlineVectorTileLayer AddBaseLayer(CartoBaseMapStyle style)
        {
            var layer = new CartoOnlineVectorTileLayer(style);
            MapView.Layers.Add(layer);
            return layer;
        }
    }
}
