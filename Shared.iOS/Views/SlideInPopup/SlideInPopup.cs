
using System;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
    public class SlideInPopup : UIView
    {
        UIView transparentArea;
        PopupView popup;
        UIView content;

        nfloat hiddenY, visibleY;

        public PopupHeader Header { get { return popup.Header; } }

        public bool IsVisible
        {
            get { return transparentArea.Alpha >= 0.5f; }
        }

        public void ShowBackButton()
        {
            Header.BackButton.Hidden = false;    
        }

        public void ShowTitle()
        {
            Header.BackButton.Hidden = true;    
        }

        public SlideInPopup()
        {
            transparentArea = new UIView();
            transparentArea.BackgroundColor = UIColor.Black;
            transparentArea.Alpha = 0.0f;
            AddSubview(transparentArea);

            popup = new PopupView();
            AddSubview(popup);

            var recognizer = new UITapGestureRecognizer((obj) =>
            {
                Hide();
            });

            transparentArea.AddGestureRecognizer(recognizer);
        }

        public override void LayoutSubviews()
        {
            nfloat x = 0;
            nfloat y = 0;
            nfloat w = Frame.Width;
            nfloat h = Frame.Height;
			
            transparentArea.Frame = new CGRect(x, y, w, h);

			hiddenY = h;
            visibleY = h - (h / 5 * 3);

            if (Device.IsLandscape || Device.IsTablet)
            {
                // Make the popup appear on the left side and full height.
                // Width of portrait iPhone 6. This number can be tweaked, but should be quite optimal
                w = 375;
                if (Device.IsTablet)
                {
                    visibleY = Frame.Height / 3 * 2;
                }
                else
                {
                    visibleY = Device.NavBarHeight + Device.StatusBarHeight;
                }
			}

            y += h;
            h = h - visibleY;

            popup.Frame = new CGRect(x, y, w, h);

            if (IsVisible)
            {
                Show();
            }

            if (content != null)
            {
                x = 0;
                y = popup.HeaderHeight;
                w = popup.Frame.Width;
                h = popup.Frame.Height - popup.HeaderHeight;

                content.Frame = new CGRect(x, y, w, h);
            }
        }

        public void SetContent(UIView content)
        {
            if (this.content != null)
            {
                this.content.RemoveFromSuperview();
                this.content = null;
            }

            this.content = content;
            popup.AddSubview(content);
            LayoutSubviews();
        }

        public void Show()
        {
            Superview.BringSubviewToFront(this);
            SlidePopupTo(visibleY);
        }

        public void Hide()
        {
            SlidePopupTo(hiddenY);
        }

        const double Duration = 0.3;

        public void SlidePopupTo(nfloat y)
        {
            UIView.Animate(Duration, delegate
            {

                popup.UpdateY(y);

                if (y.Equals(hiddenY))
                {
                    transparentArea.Alpha = 0;
                }
                else
                {
                    transparentArea.Alpha = 0.5f;
                }

            }, delegate
            {
                if (y.Equals(hiddenY))
                {
                    Superview.SendSubviewToBack(this);
                }
            });
        }

    }
}
