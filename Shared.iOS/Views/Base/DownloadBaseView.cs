
using System;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
    public class DownloadBaseView : MapBaseView
    {
		public ProgressLabel ProgressLabel { get; private set; }

		public DownloadBaseView()
        {
			ProgressLabel = new ProgressLabel();
			AddSubview(ProgressLabel);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            ProgressLabel.Frame = new CGRect(0, Frame.Height - bottomLabelHeight, Frame.Width, bottomLabelHeight);
        }
    }
}
