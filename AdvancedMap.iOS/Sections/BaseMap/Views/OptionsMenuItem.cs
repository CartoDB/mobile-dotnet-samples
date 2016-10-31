using System;
using System.Collections.Generic;
using CoreGraphics;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class OptionsMenuItem : UIView
	{
		public EventHandler<OptionEventArgs> OptionTapped;

		UIView headerContainer;
		UIView contentContainer;

		UILabel osmLabel, separator, tileTypeLabel;

		List<OptionLabel> optionLabels;

		public Section section;
		public Section Section 
		{ 
			get { return section; }
			set 
			{
				section = value;

				osmLabel.Text = section.OSM.Name.ToUpper();
				tileTypeLabel.Text = "TYPE: " + section.Type.ToString().ToUpper();

				foreach (NameValuePair option in section.Styles)
				{
					OptionLabel optionLabel = new OptionLabel(option);

					optionLabels.Add(optionLabel);
					contentContainer.AddSubview(optionLabel);
				}

				LayoutSubviews();
			}
		}

		public OptionsMenuItem()
		{
			optionLabels = new List<OptionLabel>();

			Layer.CornerRadius = 5;
			Layer.ShadowOpacity = 0.5f;
			Layer.ShadowOffset = new CGSize(-15, 20);
			Layer.ShadowRadius = 5;
			Layer.MasksToBounds = true;

			headerContainer = new UIView();
			headerContainer.BackgroundColor = Colors.AppleBlue;

			headerContainer.Layer.ShadowOpacity = 0.5f;
			headerContainer.Layer.ShadowOffset = new CGSize(0, 2);
			headerContainer.Layer.ShadowRadius = 2;

			contentContainer = new UIView();
			contentContainer.BackgroundColor = UIColor.FromRGB(240, 240, 240);
			contentContainer.ClipsToBounds = true;

			AddSubviews(contentContainer, headerContainer);

			osmLabel = new UILabel();
			osmLabel.TextColor = UIColor.White;
			osmLabel.Font = UIFont.FromName("HelveticaNeue", 14);
			osmLabel.TextAlignment = UITextAlignment.Justified;

			separator = new UILabel { BackgroundColor = UIColor.White };

			tileTypeLabel = new UILabel();
			tileTypeLabel.TextColor = UIColor.White;
			tileTypeLabel.Font = UIFont.FromName("HelveticaNeue-Light", 12);
			tileTypeLabel.TextAlignment = UITextAlignment.Center;

			headerContainer.AddSubviews(osmLabel, separator, tileTypeLabel);

			contentContainer.AddGestureRecognizer(new UITapGestureRecognizer(OnContainerTap));
		}

		void OnContainerTap(UITapGestureRecognizer recognizer)
		{
			CGPoint point = recognizer.LocationInView(contentContainer);

			foreach (OptionLabel label in optionLabels)
			{
				if (label.Frame.Contains(point) && OptionTapped != null) {
					OptionTapped(null, new OptionEventArgs { Section = this.Section, Option = label });
				}
			}
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat height = Frame.Height / 2;

			headerContainer.Frame = new CGRect(0, 0, Frame.Width, height);
			contentContainer.Frame = new CGRect(0, height, Frame.Width, Frame.Height - height);

			nfloat padding = 10;
			nfloat smallPadding = 6;

			nfloat separatorWidth = 1;
			nfloat separatorPadding = 7;

			nfloat x = padding;
			nfloat y = 0;
			nfloat w = headerContainer.Frame.Width / 2 - (2 * padding + separatorWidth);
			nfloat h = headerContainer.Frame.Height;

			osmLabel.Frame = new CGRect(x, y, w, h);

			x += w + padding;

			separator.Frame = new CGRect(x, separatorPadding, separatorWidth, h - 2 * separatorPadding);

			x += separatorWidth + padding;

			tileTypeLabel.Frame = new CGRect(x, y, w, h);

			x = padding;
			y = smallPadding;

			w = (contentContainer.Frame.Width - (padding + optionLabels.Count * padding)) / optionLabels.Count;
			h = contentContainer.Frame.Height - 2 * smallPadding;

			foreach (OptionLabel label in optionLabels)
			{
				label.Frame = new CGRect(x, y, w, h);
				x += w + padding;
			}
		}
	}

	public class OptionLabel : UILabel
	{
		public string Name { get; set; }

		public string Value { get; set; }

		public bool IsActive { get { return BackgroundColor != UIColor.White; } }

		public OptionLabel(NameValuePair option)
		{
			Name = option.Name;
			Value = option.Value;

			Text = option.Name.ToUpper();

			TextAlignment = UITextAlignment.Center;

			Font = UIFont.FromName("HelveticaNeue-Bold", 11);

			Layer.BorderWidth = 0.5f;
		}

		public void Highlight()
		{
			BackgroundColor = Colors.AppleBlue;
			Layer.BorderColor = Colors.AppleBlue.CGColor;
			TextColor = UIColor.White;
		}

		public void Normalize()
		{
			BackgroundColor = UIColor.White;
			Layer.BorderColor = UIColor.FromRGB(50, 50, 50).CGColor;
			TextColor = UIColor.FromRGB(50, 50, 50);
		}

	}
}

