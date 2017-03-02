
using System;
using Carto.Ui;
using CoreGraphics;
using UIKit;

namespace CartoMap.iOS
{
	public class TorqueView : UIView
	{
		public MapView MapView { get; private set; }

		public TorqueHistogram Histogram { get; private set; }

		public TorqueView()
		{
			MapView = new MapView();
			AddSubview(MapView);

			Histogram = new TorqueHistogram();
			AddSubview(Histogram);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			MapView.Frame = Bounds;

			nfloat padding = Histogram.Margin;

			nfloat y = Frame.Height - (Histogram.TotalHeight + padding);
			nfloat w = Frame.Width - 2 * padding;

			Histogram.Frame = new CGRect(padding, y, w, Histogram.TotalHeight);
		}
	}
}
