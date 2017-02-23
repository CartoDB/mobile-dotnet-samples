
using System;
using Carto.Ui;
using CoreGraphics;
using UIKit;

namespace CartoMap.iOS
{
	public class TorqueView : UIView
	{
		public MapView MapView { get; private set; }

		public PlayButton Button { get; private set; }

		public TorqueCounter Counter { get; private set; }

		public TorqueView()
		{
			MapView = new MapView();
			AddSubview(MapView);

			Button = new PlayButton();
			AddSubview(Button);

			Counter = new TorqueCounter();
			AddSubview(Counter);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			MapView.Frame = Bounds;

			nfloat padding = 10;

			nfloat w = 47;
			nfloat h = w;
			nfloat x = Frame.Width - (w + padding);
			nfloat y = Frame.Height - (h + padding);

			Button.Frame = new CGRect(x, y, w, h);

			w = 90;
			h = 30;
			x = Frame.Width - (w + padding);
			y = AppDelegate.NavigationBarHeight + AppDelegate.StatusBarHeight + padding;

			Counter.Frame = new CGRect(x, y, w, h);
		}
	}
}
