using System;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
    public static class ViewExtensions
    {
        public static void AddRoundShadow(this UIView view)
        {
            view.Layer.ShadowColor = UIColor.FromRGBA(0, 0, 0, 50).CGColor;
            view.Layer.ShadowOffset = new CGSize(0.0, 2.0);
            view.Layer.ShadowOpacity = 0.5f;
            view.Layer.ShadowRadius = 0.0f;
            view.Layer.MasksToBounds = false;
            view.Layer.CornerRadius = view.Frame.Width / 2;
        }
    }
}
