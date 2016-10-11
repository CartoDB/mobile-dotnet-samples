
using System;
using System.Collections.Generic;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
	public class OptionsMenu : UIView
	{
		static nfloat BoxPadding = 20;

		const double animationDuration = 0.2;

		const int hiddenAlpha = 0;
		const int showingAlpha = 150;

		nfloat Y = 80;

		CloseButton closeButton;

		public bool IsVisible { get { return Alpha == 1; } }

		public EventHandler<EventArgs> SelectionChanged;

		List<OptionsMenuBox> Boxes = new List<OptionsMenuBox>();

		public OptionsMenu()
		{
			BackgroundColor = UIColor.FromRGBA(0, 0, 0, 150);

			Alpha = 0;

			AddGestureRecognizer(new UITapGestureRecognizer(OnBackgroundTap));

			closeButton = new CloseButton();
			AddSubview(closeButton);

			closeButton.AddGestureRecognizer(new UITapGestureRecognizer(OnBackgroundTap));

			Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Size.Width, UIScreen.MainScreen.Bounds.Size.Height);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat buttonHorizontalPadding = 15;
			nfloat buttonVerticalPadding = 20;

			nfloat w = 25;
			nfloat h = w;
			nfloat y = buttonVerticalPadding;
			nfloat x = Frame.Width - (w + buttonHorizontalPadding);

			closeButton.Frame = new CGRect(x, y, w, h);
		}

		public void AddItems(string title, Dictionary<string, string> items, OptionSelectType type)
		{
			OptionsMenuBox box = new OptionsMenuBox(title, items, type);
			box.AddGestureRecognizer(new UITapGestureRecognizer(OnBoxTap));
			AddSubview(box);
			Boxes.Add(box);

			box.SelectionChanged += OnSelectionChanged;

			nfloat x = BoxPadding;
			nfloat y = Y;
			nfloat w = Frame.Width - 2 * BoxPadding;
			nfloat h = box.GetHeight();

			box.Frame = new CGRect(x, y, w, h);

			Y += h + BoxPadding;
		}

		public void UpdateItems(int index, Dictionary<string, string> items, OptionSelectType type)
		{
			Boxes[index].Update(items, type);
		}

		public void SetInitialValueOf(string label, string value)
		{
			foreach (OptionsMenuBox box in Boxes)
			{
				if (box.Title == label) {
					box.SetValue(value);
					return;
				}
			}
		}

		public string GetSelectedValueOf(string label)
		{
			foreach (OptionsMenuBox box in Boxes)
			{
				if (box.Title == label)
				{
					return box.SelectedValue;
				}
			}

			return null;
		}

		public void Enable(string label)
		{	
			UpdateStateOfBox(label, true);
		}

		public void Disable(string label)
		{
			UpdateStateOfBox(label, false);
		}

		void UpdateStateOfBox(string label, bool enabled)
		{
			foreach (OptionsMenuBox box in Boxes)
			{
				if (box.Title == label)
				{
					box.Enabled = enabled;
					return;
				}
			}
		}

		void OnSelectionChanged(object sender, EventArgs e)
		{
			if (SelectionChanged != null)
			{
				SelectionChanged(sender, e);
			}	
		}

		public void Show()
		{
			UIApplication.SharedApplication.KeyWindow.AddSubview(this);

			Animate(animationDuration, delegate { Alpha = 1; });
		}

		public void Hide()
		{
			Animate(animationDuration, delegate { Alpha = 0; }, delegate { RemoveFromSuperview(); });
		}

		void OnBackgroundTap()
		{
			Hide();
		}

		void OnBoxTap()
		{
			// Empty GestureRecognizer so tapping on box wouldn't close the menu
		}
	}

	public class CloseButton : UIButton
	{
		UIImageView icon;

		public CloseButton()
		{
			icon = new UIImageView();
			icon.Image = UIImage.FromFile("icon_close.png");
			BackgroundColor = UIColor.Black;

			AddSubview(icon);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			Layer.CornerRadius = Frame.Width / 2;

			nfloat padding = Frame.Width / 5;

			icon.Frame = new CGRect(padding, padding, Frame.Width - 2 * padding, Frame.Height - 2 * padding);
		}
	}
}

