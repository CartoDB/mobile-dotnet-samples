
using System;
using System.Collections.Generic;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
	public class OptionsMenuBox : UIView
	{
		static nfloat Padding = 5;
		static nfloat LargePadding { get { return 2 * Padding; } }

		public static nfloat ItemHeight { get { return UIScreen.MainScreen.Bounds.Size.Height / 20; } }

		UILabel title;
		List<OptionsSelect> options = new List<OptionsSelect>();

		public EventHandler<EventArgs> SelectionChanged;

		public string Title { get { return title.Text; } }

		public OptionsMenuBox(string text, Dictionary<string, string> items, OptionSelectType type)
		{
			title = new UILabel();
			title.Text = text;
			title.Font = UIFont.FromName("Helvetica", 13);

			AddSubview(title);

			title.SizeToFit();

			foreach (KeyValuePair<string, string> item in items)
			{
				OptionsSelect option = new OptionsSelect(item, type);
				option.TouchUpInside += OnOptionSelected;

				AddSubview(option);
				options.Add(option);
			}

			Layer.CornerRadius = 5;
			BackgroundColor = UIColor.White;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat x = Padding;
			nfloat y = Padding;
			nfloat w = Frame.Width - 2 * Padding;
			nfloat h = title.Frame.Height;

			title.Frame = new CGRect(x, y, w, h);

			y += h + LargePadding;

			if (options.Count == 2)
			{
				w = (w - Padding) / 2;
			}
			else {
				w = (w - 2 * Padding) / 3;
			}

			h = ItemHeight;

			for (int i = 0; i < options.Count; i++)
			{
				int counter = i + 1;

				x = GetX(counter, w);
				options[i].Frame = new CGRect(x, y, w, h);

				if (i != 0 && counter % 3 == 0)
				{
					y += h + Padding;
				}

				Console.WriteLine(options[i].Frame);
			}
		}

		public void SetValue(string value)
		{
			foreach (OptionsSelect option in options) 
			{
				if (option.Value == value) {
					option.Highlight();
					current = option;
				}
			}	
		}

		OptionsSelect current;

		void OnOptionSelected(object sender, EventArgs e)
		{
			OptionsSelect option = (OptionsSelect)sender;

			if (current == option) {
				return;
			}

			option.Highlight();

			if (current != null) 
			{ 
				current.Unhighlight(); 
			}

			current = option;

			if (SelectionChanged != null)
			{
				SelectionChanged(sender, e);
			}
		}

		nfloat GetX(int counter, nfloat width)
		{
			if (counter % 3 == 0) 
			{
				return 3 * Padding + 2 * width;
			}

			if (counter % 2 == 0)
			{
				return 2 * Padding + width;
			}

			return Padding;
		}

		public nfloat GetHeight()
		{
			int buttonArea = (int)(Math.Ceiling(Convert.ToDouble(options.Count) / 3) * (ItemHeight + Padding));
			return title.Frame.Height + buttonArea + LargePadding + Padding;
		}
	}

	public enum OptionSelectType
	{
		None,
		Style,
		Language,
		TileType,
		OSM
	}

	public class OptionsSelect : UIButton
	{
		public static UIColor AppleBlue = UIColor.FromRGB(0, 122, 255);

		UILabel title;

		public string Value { get; set; }

		public OptionSelectType Type { get; set; }

		public OptionsSelect(KeyValuePair<string, string> item, OptionSelectType type)
		{
			Value = item.Value;
			Type = type;

			title = new UILabel();
			title.Text = item.Key;
			title.TextColor = UIColor.Black;
			title.TextAlignment = UITextAlignment.Center;
			title.Font = UIFont.FromName("Helvetica", 10);

			AddSubview(title);

			Layer.BorderWidth = 0.5f;
			Layer.BorderColor = AppleBlue.CGColor;

			BackgroundColor = UIColor.White;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			title.Frame = Bounds;
		}

		public void Highlight()
		{
			title.TextColor = UIColor.White;
			BackgroundColor = AppleBlue;
		}

		public void Unhighlight()
		{
			title.TextColor = UIColor.Black;
			BackgroundColor = UIColor.White;
		}
	}
}

