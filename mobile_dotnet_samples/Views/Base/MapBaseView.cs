using System;
using System.Collections.Generic;
using Carto.Layers;
using Carto.Projections;
using Carto.Ui;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
    public class MapBaseView : UIView
    {
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
		}

        readonly List<PopupButton> buttons = new List<PopupButton>();

        public void AddButton(PopupButton button)
        {
            buttons.Add(button);
            AddSubview(button);
        }

        public CartoOnlineVectorTileLayer AddBaseLayer(CartoBaseMapStyle style)
        {
            var layer = new CartoOnlineVectorTileLayer(style);
            MapView.Layers.Add(layer);
            return layer;
        }
    }
}
