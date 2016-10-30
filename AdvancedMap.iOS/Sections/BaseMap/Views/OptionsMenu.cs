
using System;
using System.Collections.Generic;
using CoreGraphics;
using Shared;
using UIKit;

namespace AdvancedMap.iOS
{
	public class OptionsMenu : UIView
	{
		public EventHandler<OptionEventArgs> OptionTapped;

		static nfloat BoxPadding = 20;

		const double animationDuration = 0.2;

		public bool IsVisible { get { return Alpha == 1; } }

		List<OptionsMenuItem> views;

		List<Section> items;
		public List<Section> Items
		{
			get { return items; }
			set {

				items = value;

				foreach (Section section in items)
				{
					OptionsMenuItem view = new OptionsMenuItem();
					view.Section = section;
					view.AddGestureRecognizer(new UITapGestureRecognizer(OnSubviewTap));
					views.Add(view);
					AddSubview(view);

					view.OptionTapped += OnOptionTap;
				}
			}
		}

		public OptionsMenu()
		{
			BackgroundColor = UIColor.FromRGBA(0, 0, 0, 150);

			views = new List<OptionsMenuItem>();

			Alpha = 0;

			AddGestureRecognizer(new UITapGestureRecognizer(OnBackgroundTap));

			Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Size.Width, UIScreen.MainScreen.Bounds.Size.Height);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat x = BoxPadding;
			nfloat y = BoxPadding;
			nfloat w = Frame.Width - 2 * BoxPadding;
			nfloat h = 100;

			foreach (OptionsMenuItem view in views)
			{
				view.Frame = new CGRect(x, y, w, h);
				y += h + BoxPadding;
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

		void OnSubviewTap()
		{
			// Do nothing. Just catch taps
			Console.WriteLine("Subview (box) tapped");
		}

		OptionLabel current;
		void OnOptionTap(object sender, OptionEventArgs e)
		{
			if (current != null)
			{
				current.Normalize();
			}

			e.Option.Highlight();

			current = e.Option;

			if (OptionTapped != null)
			{
				OptionTapped(null, e);
			}
		}
	}

	public class OptionEventArgs : EventArgs
	{
		public Section Section { get; set; }

		public OptionLabel Option { get; set; }
	}
}

