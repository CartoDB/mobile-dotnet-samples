
using System;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
    public class PopupView : UIView
    {
        public PopupHeader Header { get; private set; }
        UIView separator;

        public nfloat HeaderHeight { get { return PopupHeader.Height; } }

        public PopupView()
        {
            BackgroundColor = UIColor.White;

            Header = new PopupHeader();
            AddSubview(Header);

            separator = new UIView();
            separator.BackgroundColor = UIColor.FromRGBA(220, 220, 220, 150);
            AddSubview(separator);
        }

        public override void LayoutSubviews()
        {
            Header.Frame = new CGRect(0, 0, Frame.Width, HeaderHeight);

            separator.Frame = new CGRect(0, HeaderHeight - 2, Frame.Width, 1);
        }
    }
}
