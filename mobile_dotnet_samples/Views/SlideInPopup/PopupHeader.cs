
using System;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
    public class PopupHeader : UIView
    {
        public static readonly nfloat Height = 40;

        public PopupBackButton BackButton { get; private set; }

        public UILabel Label { get; private set; }

        PopupCloseButton closeButton;

        public PopupHeader()
        {
            Label = new UILabel();
            Label.TextAlignment = UITextAlignment.Center;
            Label.Font = UIFont.FromName("HelveticaNeue", 11);
            Label.TextColor = Colors.CartoNavy;
            AddSubview(Label);

            BackButton = new PopupBackButton();
            BackButton.Label.Font = Label.Font;
            BackButton.Label.TextColor = Label.TextColor;
            BackButton.BackgroundColor = UIColor.White;
            AddSubview(BackButton);

            closeButton = new PopupCloseButton();
            AddSubview(closeButton);

            BackButton.Hidden = true;

			var recognizer = new UITapGestureRecognizer((obj) =>
			{
                (Superview.Superview as SlideInPopup).Hide();
			});

			closeButton.AddGestureRecognizer(recognizer);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            nfloat padding = 15.0f;

            nfloat x = padding;
            nfloat y = 0;
            nfloat w = Label.Frame.Width;
            nfloat h = Frame.Height;

            Label.Frame = new CGRect(x, y, w, h);
            BackButton.Frame = new CGRect(x, y, w, h);

            w = h;
            x = Frame.Width - w;

            closeButton.Frame = new CGRect(x, y, w, h);
        }

        public void SetText(string text)
        {
            Label.Text = text;
            Label.SizeToFit();
            LayoutSubviews();
        }
    }

	public class PopupCloseButton : UIView
	{
        UIImageView image;

        public PopupCloseButton()
        {
            image = new UIImageView();
            image.Image = UIImage.FromFile("icons/icon_close_dark.png");

            AddSubview(image);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var padding = Frame.Height / 3;

            image.Frame = new CGRect(padding, padding, Frame.Width - 2 * padding, Frame.Height - 2 * padding);
        }
	}

    public class PopupBackButton : UIView
    {
        public EventHandler<EventArgs> Click;

        UIImageView button;
        public UILabel Label { get; private set; }

        public PopupBackButton()
        {
            button = new UIImageView();
            button.Image = UIImage.FromFile("icons/icon_back_blue.png");
            AddSubview(button);

            Label = new UILabel();
            Label.Text = "BACK";
            AddSubview(Label);

            var recognizer = new UITapGestureRecognizer((obj) => {
                Click?.Invoke(this, EventArgs.Empty);
            });

            AddGestureRecognizer(recognizer);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            nfloat padding = 3;
            nfloat imagePadding = Frame.Height / 4;

            nfloat x = 0;
            nfloat y = imagePadding;
            nfloat h = Frame.Height - 2 * imagePadding;
            nfloat w = h / 2;

            button.Frame = new CGRect(x, y, w, h);

            x = button.Frame.Width + imagePadding;
            y = 0;
            w = Frame.Width - (x + padding);
            h = Frame.Height;

            Label.Frame = new CGRect(x, y, w, h);
        }
    }

}
