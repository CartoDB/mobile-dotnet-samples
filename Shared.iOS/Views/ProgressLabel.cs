
using System;
using UIKit;

namespace Shared.iOS
{
    public class ProgressLabel : UIView
    {
        UILabel label;
        UIView progressBar;

        public bool IsVisible
        {
            get { return Alpha >= 1.0f; }
        }

        public ProgressLabel()
        {
            BackgroundColor = Colors.TransparentGray;

            label = new UILabel();
            label.TextColor = UIColor.White;
            label.Font = UIFont.FromName("HelveticaNeue-Bold", 12);
            label.TextAlignment = UITextAlignment.Center;
            AddSubview(label);

            progressBar = new UIView();
            progressBar.BackgroundColor = Colors.AppleBlue;
            AddSubview(progressBar);

            Alpha = 0.0f;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            label.Frame = Bounds;
        }

        public void Update(string text, float progress)
        {
            Show();

            Update(text);
            UpdateProgress(progress);
        }

        public void Update(string text)
        {
            label.Text = text;
        }

        public void UpdateProgress(float progress)
        {
            var w = (Frame.Width * progress) / 100;
            var h = 3;
            var y = Frame.Height - h;
            var x = 0;

            progressBar.Frame = new CoreGraphics.CGRect(x, y, w, h);
        }

        public void Show()
        {
            AnimateAlpha(1);
        }

        public void Hide()
        {
            AnimateAlpha(0);
        }

        void AnimateAlpha(float alpha)
        {
            Animate(0.3, delegate {
                Alpha = alpha;
            });
        }
    }
}
